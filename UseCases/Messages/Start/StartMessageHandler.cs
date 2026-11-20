using Bot.Core;
using Bot.Core.Messages;
using Bot.Pages;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.UseCases.Messages.Start;

internal sealed class StartMessageHandler(ITelegramBotClient bot)
    : IBotMessageHandler<StartMessage>
{
    public async Task Handle(
        StartMessage command,
        CancellationToken cancellationToken)
    {
        var chat = command.Message.Chat;
        if (chat.Type != ChatType.Private || command.Message.From is not { } user)
        {
            await bot.SendMessage(chat.Id, "برای دیدن صفحهٔ حساب، بات را در گفت‌وگوی خصوصی باز کن.",
                cancellationToken: cancellationToken);
            return;
        }

        await using var logo = AssemblyReference.Assembly.GetManifestResourceStream("Bot.DashboardLogo.png")
                               ?? throw new InvalidOperationException("The dashboard logo is missing.");
        await bot.SendPhoto(
            chat.Id,
            InputFile.FromStream(logo, "logo.png"),
            caption: DashboardPage.Caption(user),
            parseMode: ParseMode.Html,
            replyMarkup: DashboardPage.Keyboards,
            cancellationToken: cancellationToken);
    }
}
