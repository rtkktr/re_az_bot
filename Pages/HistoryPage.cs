using Bot.Core;
using Bot.Core.Updates;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Pages;

internal sealed class HistoryPage(User user, long? adminId) : Page(user, adminId)
{
    protected override List<List<InlineKeyboardButton>>? SetInlineKeyboardButtonListForUsers()
    {
        return
        [
            [
                InlineKeyboardButton.WithCallbackData("بازگشت به صفحهٔ اصلی", BotCallbackQueryConstants.BackToHome)
            ]
        ];
    }

    protected override List<List<InlineKeyboardButton>>? SetInlineKeyboardButtonListForAdmins()
    {
        return [];
    }

    protected override string? SetCaptionForUsers()
    {
        return "<b>تاریخچهٔ خرید</b>\n\nتاریخچهٔ خرید فعلاً در دسترس نیست.";
    }

    protected override string? SetCaptionForAdmins()
    {
        return "";
    }
}