using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DesafioTjRjErlimar.DatabaseAdapter;

public class DatabaseContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;

    public DatabaseContext(DbContextOptions options, ILoggerFactory loggerFactory) : base(options)
    {
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    }

    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
#if !DEBUG
            .UseLoggerFactory(_loggerFactory);
#else
            .UseLoggerFactory(_loggerFactory)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
#endif
    }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}