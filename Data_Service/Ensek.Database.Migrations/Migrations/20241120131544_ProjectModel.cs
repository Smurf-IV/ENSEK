using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ensek.Database.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class ProjectModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Ensek");

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "Ensek",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "FailedMeterReadings",
                schema: "Ensek",
                columns: table => new
                {
                    FailedMeterReadingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TsIdentifier = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    ReadingDateTime = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    MeterReadValue = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedMeterReadings", x => x.FailedMeterReadingId);
                });

            migrationBuilder.CreateTable(
                name: "MeterReadings",
                schema: "Ensek",
                columns: table => new
                {
                    MeterReadingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ReadingDateTime = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    ReadingDateTimeUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MeterReadValue = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadings", x => x.MeterReadingId);
                    table.ForeignKey(
                        name: "FK_MeterReadings_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Ensek",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountId",
                schema: "Ensek",
                table: "Accounts",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadings_AccountId_ReadingDateTimeUTC",
                schema: "Ensek",
                table: "MeterReadings",
                columns: new[] { "AccountId", "ReadingDateTimeUTC" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FailedMeterReadings",
                schema: "Ensek");

            migrationBuilder.DropTable(
                name: "MeterReadings",
                schema: "Ensek");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "Ensek");
        }
    }
}
