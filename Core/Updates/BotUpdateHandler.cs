using Bot.UseCases.CallbackQueries.BackToHome;
using Bot.UseCases.CallbackQueries.Buy;
using Bot.UseCases.CallbackQueries.History;
using MediatR;
using Telegram.Bot.Types;

namespace Bot.Core.Updates;

internal sealed class BotUpdateHandler(ISender sender)
{
    public Task OnUpdate(Update update, CancellationToken cancellationToken)
    {
        if (update.CallbackQuery is not { } query)
            return Task.CompletedTask;

        return query.Data switch
        {
            BotCallbackQueryConstants.BackToHome =>
                sender.Send(new BackToHomeCallbackQuery {CallbackQuery = query}, cancellationToken),
            BotCallbackQueryConstants.Buy =>
                sender.Send(new BuyCallbackQuery {CallbackQuery = query}, cancellationToken),
            BotCallbackQueryConstants.History =>
                sender.Send(new HistoryCallbackQuery {CallbackQuery = query}, cancellationToken),
            _ => Task.CompletedTask
        };
    }
}
