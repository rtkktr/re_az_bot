using MediatR;

namespace Bot.Core.Updates;

public interface IBotCallbackQueryHandler<in TQuery> : IRequestHandler<TQuery>
    where TQuery : BotCallbackQuery { }