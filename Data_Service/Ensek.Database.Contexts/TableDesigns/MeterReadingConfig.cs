using Ensek.Database.Tables;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ensek.Database.Contexts.TableDesigns;

internal class MeterReadingConfig : IEntityTypeConfiguration<MeterReading>
{
    public void Configure(EntityTypeBuilder<MeterReading> builder)
    {
        builder.Property(b => b.AccountId)
            .IsRequired();

        builder.Property(b => b.ReadingDateTime)
            .IsRequired()
            .HasMaxLength(24);

        builder.Property(b => b.ReadingDateTimeUTC)
            .IsRequired();

        builder.Property(b => b.MeterReadValue)
            .IsRequired();

        // Secondary index (with AccountId) to ensure values are only added once
        builder.HasIndex(p => new { p.AccountId, p.ReadingDateTimeUTC })
            .IsUnique(true);
    }
}