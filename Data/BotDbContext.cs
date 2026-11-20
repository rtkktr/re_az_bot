using Microsoft.EntityFrameworkCore;

namespace Bot.Data;

internal sealed class BotDbContext(DbContextOptions<BotDbContext> options) : DbContext(options);
