using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TopUpAPI.Migrations
{
    /// <inheritdoc />
    public partial class LogintableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoginModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginModels", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvailableBalance", "IsVerified", "TotalTopUpLimit", "Username" },
                values: new object[,]
                {
                    { 201, 1450m, true, 3000m, "Prakash" },
                    { 202, 2000m, false, 3000m, "Pavithra" }
                });

            migrationBuilder.InsertData(
                table: "Beneficiaries",
                columns: new[] { "Id", "MonthlyTopUpLimit", "Nickname", "UserId" },
                values: new object[,]
                {
                    { 11, 500m, "William", 201 },
                    { 12, 1000m, "Robert", 202 }
                });

            migrationBuilder.InsertData(
                table: "BeneficiaryTopUpDetails",
                columns: new[] { "Id", "Amount", "BeneficiaryId", "Charge", "MonthWise", "UserId", "YearWise" },
                values: new object[,]
                {
                    { 121, 100m, 11, 1m, 10, 201, 2024 },
                    { 122, 75m, 12, 1m, 10, 202, 2024 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginModels");

            migrationBuilder.DeleteData(
                table: "BeneficiaryTopUpDetails",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "BeneficiaryTopUpDetails",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Beneficiaries",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Beneficiaries",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 202);
        }
    }
}
