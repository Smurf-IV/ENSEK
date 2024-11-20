using Ensek.Database.Tables;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ensek.Database.Contexts.TableDesigns;

internal class AccountConfig : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(b => b.AccountId)
            .IsRequired();

        // External Foreign Key / Index
        builder.HasIndex(p => new { p.AccountId})
            .IsUnique(true);

        builder.Property(b => b.FirstName)
                .IsRequired()
                .HasMaxLength(128); // TODO: What could the max FirstName length be ?

        builder.Property(b => b.LastName)
            .IsRequired()
            .HasMaxLength(128); // TODO: What could the max LastName length be ?

        builder.HasMany(b => b.MeterReadings)
            .WithOne()
            .HasForeignKey("AccountId");
    }
}