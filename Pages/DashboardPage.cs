using System.Net;
using Bot.Core.Updates;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Pages;

public static class DashboardPage
{
    public static InlineKeyboardMarkup Keyboards { get; } = new(
        InlineKeyboardButton.WithCallbackData("خرید VPN", BotCallbackQueryConstants.Buy),
        InlineKeyboardButton.WithCallbackData("تاریخچهٔ خرید", BotCallbackQueryConstants.History)
    );
    
    public static string Caption(User user)
    {
        var name = WebUtility.HtmlEncode($"{user.FirstName} {user.LastName}".Trim());
        var username = user.Username is { Length: > 0 } value ? $"@{WebUtility.HtmlEncode(value)}" : "ثبت نشده";

        return $"""
                <b>حساب کاربری رعاض</b>

                نام · {name}
                نام کاربری · {username}
                شناسهٔ تلگرام · <code>{user.Id}</code>
                """;
    }
}