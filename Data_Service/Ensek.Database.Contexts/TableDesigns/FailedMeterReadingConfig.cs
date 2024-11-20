using Ensek.Database.Tables;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ensek.Database.Contexts.TableDesigns;

internal class FailedMeterReadingConfig : IEntityTypeConfiguration<FailedMeterReading>
{
    public void Configure(EntityTypeBuilder<FailedMeterReading> builder)
    {
        builder.Property(b => b.TsIdentifier)
            .IsRequired()
            .ValueGeneratedNever();

        //builder.Property(b => b.FailedMeterReadingId)
        //.IsRequired()

        builder.Property(b => b.ReadingDateTime)
            //.IsRequired()
            .HasMaxLength(24);

        //builder.Property(b => b.MeterReadValue)
        //    .IsRequired();

    }
}