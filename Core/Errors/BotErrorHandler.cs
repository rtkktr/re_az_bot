using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Bot.Core.Errors;

internal sealed class BotErrorHandler(ITelegramBotClient bot)
{
    public async Task OnError(Exception exception, HandleErrorSource source)
    {
        Console.WriteLine(exception);
    }
}