using System.Net;
using Bot.Core.Updates;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Pages;

public static class DashboardPage
{
    private static InlineKeyboardMarkup UserKeyboard { get; } = new(
        InlineKeyboardButton.WithCallbackData("خرید VPN", BotCallbackQueryConstants.Buy),
        InlineKeyboardButton.WithCallbackData("تاریخچهٔ خرید", BotCallbackQueryConstants.History)
    );

    public static (string Caption, InlineKeyboardMarkup? Keyboard) ForUser(User user, long? adminId)
    {
        var name = WebUtility.HtmlEncode($"{user.FirstName} {user.LastName}".Trim());
        var username = user.Username is { Length: > 0 } value ? $"@{WebUtility.HtmlEncode(value)}" : "ثبت نشده";
        var accountDetails = $"""
            نام · {name}
            نام کاربری · {username}
            شناسهٔ تلگرام · <code>{user.Id}</code>
            """;

        if (user.Id == adminId)
            return ($"<b>پنل مدیریت رعاض</b>\n\n{accountDetails}\nنقش · مدیر", null);

        return ($"<b>حساب کاربری رعاض</b>\n\n{accountDetails}", UserKeyboard);
    }
}
