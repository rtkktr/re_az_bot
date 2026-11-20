using MediatR;
using Telegram.Bot.Types;

namespace Bot.Core.Messages;

public abstract class BotMessage : IRequest
{
    public required Message Message { get; init; }
}