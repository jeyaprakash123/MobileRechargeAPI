using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopUpAPI.Migrations
{
    /// <inheritdoc />
    public partial class MonthlytopupTotalColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyTopUpTotal",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyTopUpTotal",
                table: "Users");
        }
    }
}
