using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopUpAPI.Migrations
{
    /// <inheritdoc />
    public partial class BeneficiaryTopUpDetailsTableCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeneficiaryTopUpDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Charge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyTopUpLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyTopUpTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BeneficiaryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneficiaryTopUpDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeneficiaryTopUpDetails_Beneficiaries_BeneficiaryId",
                        column: x => x.BeneficiaryId,
                        principalTable: "Beneficiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiaryTopUpDetails_BeneficiaryId",
                table: "BeneficiaryTopUpDetails",
                column: "BeneficiaryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeneficiaryTopUpDetails");
        }
    }
}
