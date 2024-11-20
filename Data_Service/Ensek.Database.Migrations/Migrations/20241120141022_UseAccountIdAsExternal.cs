using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ensek.Database.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UseAccountIdAsExternal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowId",
                schema: "Ensek",
                table: "Accounts");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                schema: "Ensek",
                table: "Accounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                schema: "Ensek",
                table: "Accounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "RowId",
                schema: "Ensek",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
