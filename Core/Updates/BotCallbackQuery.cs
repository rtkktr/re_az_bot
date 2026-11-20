using MediatR;
using Telegram.Bot.Types;

namespace Bot.Core.Updates;

public abstract class BotCallbackQuery : IRequest
{
    public required CallbackQuery CallbackQuery { get; init; }
}