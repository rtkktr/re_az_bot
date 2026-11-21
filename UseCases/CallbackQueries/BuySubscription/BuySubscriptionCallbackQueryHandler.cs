using Bot.Core;
using Bot.Core.Updates;
using Bot.Pages;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace Bot.UseCases.CallbackQueries.BuySubscription;

internal sealed class BuySubscriptionCallbackQueryHandler(ITelegramBotClient bot, IOptions<BotOptions> options) 
    : IBotCallbackQueryHandler<BuySubscriptionCallbackQuery>
{
    public async Task Handle(
        BuySubscriptionCallbackQuery request,
        CancellationToken cancellationToken)
    {
        await bot.AnswerCallbackQuery(request.CallbackQuery.Id, cancellationToken: cancellationToken);

        if (request.CallbackQuery.Message is not { Chat.Type: ChatType.Private, BusinessConnectionId: null, Photo.Length: > 0 } message ||
            message.Chat.Id != request.CallbackQuery.From.Id)
            return;
        
        var page = new BuySubscriptionPage(request.CallbackQuery.From, options.Value.AdminId);
        
        try
        {
            await bot.EditMessageCaption(
                message.Chat.Id,
                message.Id,
                page.GetCaption(),
                ParseMode.Html,
                replyMarkup: page.GetInlineKeyboardMarkup(),
                cancellationToken: cancellationToken);
        }
        catch (ApiRequestException exception) when (exception.ErrorCode == 400 &&
                                                    exception.Message.Contains("message is not modified", StringComparison.OrdinalIgnoreCase))
        {
            // The requested page is already visible.
        }
    }
}