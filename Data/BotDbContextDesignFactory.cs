using Bot.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bot.Data;

internal sealed class BotDbContextDesignFactory : IDesignTimeDbContextFactory<BotDbContext>
{
    public BotDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("BOT_DB_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connectionString))
            connectionString = new BotOptions().DatabaseConnectionString;

        var options = new DbContextOptionsBuilder<BotDbContext>().UseSqlite(connectionString).Options;
        return new BotDbContext(options);
    }
}
