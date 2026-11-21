using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Bot.Core.Errors;

#pragma warning disable CS9113 // Parameter is unread.
internal sealed class BotErrorHandler(ITelegramBotClient bot)
#pragma warning restore CS9113 // Parameter is unread.
{
    public async Task OnError(Exception exception, HandleErrorSource source)
    {
        Console.WriteLine(exception);
    }
}