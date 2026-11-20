using Bot;
using Bot.Core;
using Bot.Core.Errors;
using Bot.Core.Messages;
using Bot.Core.Updates;
using Bot.Data;
using DotNetEnv;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

Env.NoClobber().TraversePath().Load();

var token = Environment.GetEnvironmentVariable("BOT_TOKEN");
if (string.IsNullOrWhiteSpace(token))
{
    Console.Error.WriteLine(
        $"BOT_TOKEN is missing. .env was looked up in: {Environment.CurrentDirectory}");
    return;
}

using var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;
var bot = new TelegramBotClient(token, cancellationToken: cancellationToken);

var connectionString = Environment.GetEnvironmentVariable("BOT_DB_CONNECTION_STRING");
if (string.IsNullOrWhiteSpace(connectionString))
    connectionString = "Data Source=bot.db";

var services = new ServiceCollection();
services.AddSingleton<ITelegramBotClient>(bot);
services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly));
services.AddDbContextFactory<BotDbContext>(options => options.UseSqlite(connectionString));

await using var serviceProvider = services.BuildServiceProvider();
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
