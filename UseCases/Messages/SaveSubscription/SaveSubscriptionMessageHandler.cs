using System.Globalization;
using Bot.Core;
using Bot.Core.Messages;
using Bot.Data;
using Bot.Pages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.UseCases.Messages.SaveSubscription;

internal sealed class SaveSubscriptionMessageHandler(
    ITelegramBotClient bot, IOptions<BotOptions> options, IDbContextFactory<BotDbContext> databaseFactory)
    : IBotMessageHandler<SaveSubscriptionMessage>
{
    private const int SqliteUniqueConstraint = 2067;

    public async Task Handle(SaveSubscriptionMessage request, CancellationToken cancellationToken)
    {
        var message = request.Message;
        if (message.Chat.Type != ChatType.Private || message.From is not { } user ||
            user.Id != options.Value.AdminId || user.Id != message.Chat.Id ||
            message.ReplyToMessage is not { From.Id: var promptAuthorId, Text: AddSubscriptionPage.Prompt } prompt ||
            promptAuthorId != bot.BotId)
            return;

        if (Parse(message.Text) is not { } values)
        {
            await bot.SendMessage(message.Chat.Id, "در پاسخ به پیام درخواست، دو عدد صحیح مثبت بفرست. مثال · 30 150",
                replyParameters: new ReplyParameters { MessageId = prompt.Id }, cancellationToken: cancellationToken);
            return;
        }

        var plan = new SubscriptionPlan
        {
            DataLimitGb = values.DataLimitGb,
            MaxUsers = 2,
            PriceTomans = values.PriceInThousands * 1_000L,
            AdminChatId = message.Chat.Id,
            PromptMessageId = prompt.Id
        };

        await using var database = databaseFactory.CreateDbContext();
        database.SubscriptionPlans.Add(plan);
        try
        {
            await database.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException error) when (error.InnerException is SqliteException
            { SqliteExtendedErrorCode: SqliteUniqueConstraint })
        {
            await bot.SendMessage(message.Chat.Id, "برای این درخواست قبلاً یک اشتراک ثبت شده است.",
                cancellationToken: cancellationToken);
            return;
        }

        var price = plan.PriceTomans.ToString("N0", CultureInfo.InvariantCulture);
        await bot.SendMessage(message.Chat.Id, $"اشتراک ثبت شد ✅\n{plan.DataLimitGb} گیگ · دو کاربره · {price} تومان",
            cancellationToken: cancellationToken);
    }

    private static (int DataLimitGb, int PriceInThousands)? Parse(string? text)
    {
        if (text is null)
            return null;

        var normalized = new string(text.Select(character => character switch
        {
            >= '۰' and <= '۹' => (char)('0' + character - '۰'),
            >= '٠' and <= '٩' => (char)('0' + character - '٠'),
            _ => character
        }).ToArray());
        var parts = normalized.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2 ||
            !int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out var dataLimitGb) ||
            !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var priceInThousands) ||
            dataLimitGb <= 0 || priceInThousands <= 0)
            return null;

        return (dataLimitGb, priceInThousands);
    }
}
