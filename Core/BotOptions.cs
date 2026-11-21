namespace Bot.Core;

internal sealed class BotOptions
{
    public string Token { get; set; } = string.Empty;
    public string DatabaseConnectionString { get; set; } = "Data Source=bot.db";
    public long? AdminId { get; set; }
}
