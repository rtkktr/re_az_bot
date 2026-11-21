using System.Net;
using Bot.Core;
using Bot.Core.Updates;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Pages;

internal sealed class DashboardPage(User user, long? adminId = null) : Page(user, adminId)
{
    protected override List<List<InlineKeyboardButton>>? SetInlineKeyboardButtonListForUsers()
    {
        return
        [
            [
                InlineKeyboardButton.WithCallbackData("خرید اشتراک", BotCallbackQueryConstants.BuySubscription),
                InlineKeyboardButton.WithCallbackData("تاریخچهٔ سفارشات", BotCallbackQueryConstants.History)
            ]
        ];
    }

    protected override List<List<InlineKeyboardButton>>? SetInlineKeyboardButtonListForAdmins()
    {
        return
        [
            [InlineKeyboardButton.WithCallbackData("افزودن اشتراک", BotCallbackQueryConstants.AddSubscription)]
        ];
    }

    protected override string? SetCaptionForUsers()
    {
        var name = WebUtility.HtmlEncode($"{User.FirstName} {User.LastName}".Trim());
        var username = User.Username is { Length: > 0 } value ? $"@{WebUtility.HtmlEncode(value)}" : "ثبت نشده";
        var accountDetails = $"""
                              نام · {name}
                              نام کاربری · {username}
                              شناسهٔ تلگرام · <code>{User.Id}</code>
                              """;
        return $"<b>حساب کاربری رعاض</b>\n\n{accountDetails}";
    }

    protected override string? SetCaptionForAdmins()
    {
        var name = WebUtility.HtmlEncode($"{User.FirstName} {User.LastName}".Trim());
        var username = User.Username is { Length: > 0 } value ? $"@{WebUtility.HtmlEncode(value)}" : "ثبت نشده";
        var accountDetails = $"""
                              نام · {name}
                              نام کاربری · {username}
                              شناسهٔ تلگرام · <code>{User.Id}</code>
                              """;
        return $"<b>پنل مدیریت رعاض</b>\n\n{accountDetails}\nنقش · مدیر";
    }
}
