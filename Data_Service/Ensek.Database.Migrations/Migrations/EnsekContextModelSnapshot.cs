﻿// <auto-generated />
using System;
using Ensek.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ensek.Database.Migrations.Migrations
{
    [DbContext(typeof(EnsekContext))]
    partial class EnsekContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Ensek")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ensek.Database.Tables.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("AccountId");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Accounts", "Ensek");
                });

            modelBuilder.Entity("Ensek.Database.Tables.FailedMeterReading", b =>
                {
                    b.Property<int>("FailedMeterReadingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FailedMeterReadingId"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<int?>("MeterReadValue")
                        .HasColumnType("int");

                    b.Property<string>("ReadingDateTime")
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<long>("TsIdentifier")
                        .HasColumnType("bigint");

                    b.HasKey("FailedMeterReadingId");

                    b.ToTable("FailedMeterReadings", "Ensek");
                });

            modelBuilder.Entity("Ensek.Database.Tables.MeterReading", b =>
                {
                    b.Property<int>("MeterReadingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MeterReadingId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<long>("MeterReadValue")
                        .HasColumnType("bigint");

                    b.Property<string>("ReadingDateTime")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<DateTime>("ReadingDateTimeUTC")
                        .HasColumnType("datetime2");

                    b.HasKey("MeterReadingId");

                    b.HasIndex("AccountId", "ReadingDateTimeUTC")
                        .IsUnique();

                    b.ToTable("MeterReadings", "Ensek");
                });

            modelBuilder.Entity("Ensek.Database.Tables.MeterReading", b =>
                {
                    b.HasOne("Ensek.Database.Tables.Account", null)
                        .WithMany("MeterReadings")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ensek.Database.Tables.Account", b =>
                {
                    b.Navigation("MeterReadings");
                });
#pragma warning restore 612, 618
        }
    }
}
