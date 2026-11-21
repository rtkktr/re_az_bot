namespace Bot.Data;

internal sealed class SubscriptionPlan
{
    public int Id { get; set; }
    public int DataLimitGb { get; set; }
    public int MaxUsers { get; set; }
    public long PriceTomans { get; set; }
    public long AdminChatId { get; set; }
    public int PromptMessageId { get; set; }
}
