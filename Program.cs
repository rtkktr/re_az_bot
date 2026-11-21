using Bot.Core;
using Bot.Core.Errors;
using Bot.Core.Messages;
using Bot.Core.Updates;
using Bot.Data;
using DotNetEnv;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

Env.NoClobber().TraversePath().Load();

using var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;

var services = new ServiceCollection();
services.AddOptions<BotOptions>()
    .Configure(options =>
    {
        options.Token = Environment.GetEnvironmentVariable("BOT_TOKEN") ?? string.Empty;

        var connectionString = Environment.GetEnvironmentVariable("BOT_DB_CONNECTION_STRING");
        if (!string.IsNullOrWhiteSpace(connectionString))
            options.DatabaseConnectionString = connectionString;

        if (long.TryParse(Environment.GetEnvironmentVariable("ADMIN_Id"), out var adminId))
            options.AdminId = adminId;
    })
    .Validate(options => !string.IsNullOrWhiteSpace(options.Token), "BOT_TOKEN is missing.");

services.AddSingleton(provider => new TelegramBotClient(
    provider.GetRequiredService<IOptions<BotOptions>>().Value.Token, cancellationToken: cancellationToken));
services.AddSingleton<ITelegramBotClient>(provider => provider.GetRequiredService<TelegramBotClient>());
services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly));
services.AddDbContextFactory<BotDbContext>((provider, options) =>
    options.UseSqlite(provider.GetRequiredService<IOptions<BotOptions>>().Value.DatabaseConnectionString));

await using var serviceProvider = services.BuildServiceProvider();
TelegramBotClient bot;
try
{
    bot = serviceProvider.GetRequiredService<TelegramBotClient>();
}
catch (OptionsValidationException exception)
{
    Console.Error.WriteLine($"{exception.Message} .env was looked up in: {Environment.CurrentDirectory}");
    return;
}

var databaseFactory = serviceProvider.GetRequiredService<IDbContextFactory<BotDbContext>>();
await using (var database = databaseFactory.CreateDbContext())
{
    database.Database.OpenConnection();
}

var me = await bot.GetMe();
var sender = serviceProvider.GetRequiredService<ISender>();
var messageHandler = new BotMessageHandler(sender);
var updateHandler = new BotUpdateHandler(sender);
var errorHandler = new BotErrorHandler(bot);
bot.OnError += errorHandler.OnError;
bot.OnMessage += (message, _) => messageHandler.OnMessage(message, cancellationToken);
bot.OnUpdate += update => updateHandler.OnUpdate(update, cancellationToken);

Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel();
