using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Core;

public abstract class Page(User user, long? adminId = null)
{
    protected User User { get; } = user;

    public InlineKeyboardMarkup GetInlineKeyboardMarkup()
    {
        var rows = new List<List<InlineKeyboardButton>>();
        rows.AddRange(SetInlineKeyboardButtonListForUsers() ?? []);
        if (User.Id == adminId)
            rows.AddRange(SetInlineKeyboardButtonListForAdmins() ?? []);

        return new InlineKeyboardMarkup(rows);
    }

    public string GetCaption()
    {
        if (User.Id != adminId) return SetCaptionForUsers() ?? string.Empty;
        return SetCaptionForAdmins() ??  string.Empty;
    }

    protected abstract List<List<InlineKeyboardButton>>? SetInlineKeyboardButtonListForUsers();
    protected abstract List<List<InlineKeyboardButton>>? SetInlineKeyboardButtonListForAdmins();
    protected abstract string? SetCaptionForUsers();
    protected abstract string? SetCaptionForAdmins();
}
