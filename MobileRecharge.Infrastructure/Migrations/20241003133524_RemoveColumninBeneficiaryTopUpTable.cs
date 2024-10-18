using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopUpAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumninBeneficiaryTopUpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "BeneficiaryTopUpDetails");

            migrationBuilder.DropColumn(
                name: "MonthlyTopUpTotal",
                table: "BeneficiaryTopUpDetails");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "BeneficiaryTopUpDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "BeneficiaryTopUpDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "BeneficiaryTopUpDetails");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "BeneficiaryTopUpDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "BeneficiaryTopUpDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyTopUpTotal",
                table: "BeneficiaryTopUpDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
