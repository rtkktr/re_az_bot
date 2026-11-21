using Bot.Core;
using Bot.Core.Updates;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Pages;

internal sealed class BuySubscriptionPage(User user, long? adminId) : Page(user, adminId)
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
        return "<b>خرید VPN</b>\n\nفروش هنوز فعال نشده است.";
    }

    protected override string? SetCaptionForAdmins()
    {
        return "";
    }
}