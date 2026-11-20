using Bot.Core.Updates;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Pages;

public abstract class BuyPage
{
    
    public static InlineKeyboardMarkup Keyboards { get; } = new(
        InlineKeyboardButton.WithCallbackData("بازگشت به صفحهٔ اصلی", BotCallbackQueryConstants.BackToHome)
    );
    
    public static string Caption()
    {
        return "<b>خرید VPN</b>\n\nفروش هنوز فعال نشده است.";
    }
}