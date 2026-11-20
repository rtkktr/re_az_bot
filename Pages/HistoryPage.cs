using Bot.Core.Updates;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Pages;

public abstract class HistoryPage
{
    
    public static InlineKeyboardMarkup Keyboards { get; } = new(
        InlineKeyboardButton.WithCallbackData("بازگشت به صفحهٔ اصلی", BotCallbackQueryConstants.BackToHome)
    );
    
    public static string Caption()
    {
        return "<b>تاریخچهٔ خرید</b>\n\nتاریخچهٔ خرید فعلاً در دسترس نیست.";
    }
}