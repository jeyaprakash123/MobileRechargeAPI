using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopUpAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedBalanceColumnInUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthlyTopUpLimit",
                table: "Users",
                newName: "TotalTopUpLimit");

            migrationBuilder.AddColumn<decimal>(
                name: "AvailableBalance",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableBalance",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "TotalTopUpLimit",
                table: "Users",
                newName: "MonthlyTopUpLimit");
        }
    }
}
