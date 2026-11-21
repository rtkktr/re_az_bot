using Bot.Core;
using Bot.Core.Updates;
using Bot.Pages;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.UseCases.CallbackQueries.AddSubscription;

internal sealed class AddSubscriptionCallbackQueryHandler(ITelegramBotClient bot, IOptions<BotOptions> options)
    : IBotCallbackQueryHandler<AddSubscriptionCallbackQuery>
{
    public async Task Handle(
        AddSubscriptionCallbackQuery request,
        CancellationToken cancellationToken)
    {
        await bot.AnswerCallbackQuery(request.CallbackQuery.Id, cancellationToken: cancellationToken);

        if (request.CallbackQuery.From.Id != options.Value.AdminId)
            return;

        if (request.CallbackQuery.Message is not { Chat.Type: ChatType.Private, BusinessConnectionId: null, Photo.Length: > 0 } message ||
            message.Chat.Id != request.CallbackQuery.From.Id)
            return;

        await bot.SendMessage(message.Chat.Id, AddSubscriptionPage.Prompt,
            replyMarkup: new ForceReplyMarkup { InputFieldPlaceholder = "30 150" },
            cancellationToken: cancellationToken);
    }
}
