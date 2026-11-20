using MediatR;

namespace Bot.Core.Messages;

public interface IBotMessageHandler<in TMessage> : IRequestHandler<TMessage>
    where TMessage : BotMessage { }