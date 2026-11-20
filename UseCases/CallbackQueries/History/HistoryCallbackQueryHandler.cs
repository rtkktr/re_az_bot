using Bot.Core.Updates;
using Bot.Pages;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace Bot.UseCases.CallbackQueries.History;

internal sealed class HistoryCallbackQueryHandler(ITelegramBotClient bot)
    : IBotCallbackQueryHandler<HistoryCallbackQuery>
{
    public async Task Handle(
        HistoryCallbackQuery request,
        CancellationToken cancellationToken)
    {
        await bot.AnswerCallbackQuery(request.CallbackQuery.Id, cancellationToken: cancellationToken);

        if (request.CallbackQuery.Message is not { Chat.Type: ChatType.Private, BusinessConnectionId: null, Photo.Length: > 0 } message ||
            message.Chat.Id != request.CallbackQuery.From.Id)
            return;

        try
        {
            await bot.EditMessageCaption(
                message.Chat.Id,
                message.Id,
                HistoryPage.Caption(),
                ParseMode.Html,
                replyMarkup: HistoryPage.Keyboards,
                cancellationToken: cancellationToken);
        }
        catch (ApiRequestException exception) when (exception.ErrorCode == 400 &&
                                                    exception.Message.Contains("message is not modified", StringComparison.OrdinalIgnoreCase))
        {
            // The requested page is already visible.
        }
    }
}