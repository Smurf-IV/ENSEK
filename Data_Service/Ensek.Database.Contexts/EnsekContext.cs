using System.Diagnostics.CodeAnalysis;

using Ensek.Database.Contexts.Context;
using Ensek.Database.Contexts.Initializers;
using Ensek.Database.Contexts.TableDesigns;
using Ensek.Database.Tables;

using Microsoft.EntityFrameworkCore;

namespace Ensek.Database.Contexts;

public class EnsekContext : BaseDbContext
{
    public EnsekContext(DbContextOptions<EnsekContext> options, IDatabaseInitializer initializer)
        : base(options, initializer)
    {
    }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<MeterReading> MeterReadings { get; set; }

    public DbSet<FailedMeterReading> FailedMeterReadings { get; set; }

    protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(@"Ensek");

        modelBuilder.ApplyConfiguration(new AccountConfig());
        modelBuilder.ApplyConfiguration(new MeterReadingConfig());
        modelBuilder.ApplyConfiguration(new FailedMeterReadingConfig());

        base.OnModelCreating(modelBuilder);
    }
}