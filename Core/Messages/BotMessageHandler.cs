using Bot.UseCases.Messages.Start;
using MediatR;
using Telegram.Bot.Types;

namespace Bot.Core.Messages;

internal sealed class BotMessageHandler(ISender sender)
{
    public Task OnMessage(Message message, CancellationToken cancellationToken)
    {
        if (message.BusinessConnectionId is not null)
            return Task.CompletedTask;

        return message.Text switch
        {
            BotMessageConstants.Start => sender.Send(new StartMessage { Message = message }, cancellationToken),
            _ => Task.CompletedTask
        };
    }
}
