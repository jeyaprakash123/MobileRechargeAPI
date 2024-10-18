using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopUpAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateBeneficiaryTopUpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "BeneficiaryTopUpDetails",
                newName: "YearWise");

            migrationBuilder.RenameColumn(
                name: "Month",
                table: "BeneficiaryTopUpDetails",
                newName: "MonthWise");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YearWise",
                table: "BeneficiaryTopUpDetails",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "MonthWise",
                table: "BeneficiaryTopUpDetails",
                newName: "Month");
        }
    }
}
