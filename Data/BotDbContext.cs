using Microsoft.EntityFrameworkCore;

namespace Bot.Data;

internal sealed class BotDbContext(DbContextOptions<BotDbContext> options) : DbContext(options)
{
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubscriptionPlan>()
            .HasIndex(plan => new { plan.AdminChatId, plan.PromptMessageId })
            .IsUnique();
    }
}
