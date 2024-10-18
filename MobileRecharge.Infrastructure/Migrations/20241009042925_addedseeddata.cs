using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TopUpAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedseeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LoginModels",
                columns: new[] { "Id", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "$2a$11$uOEdhHs.vGGfyyTBXhxS3uW10.GU4cN7ax43TXYsz62AGxM5qxjPe", "Prakash" },
                    { 2, "$2a$11$5OrC0nurkwVhqjpwsIxYi.wqxPktD3sDuxgcGYhCvcMFIAvnHvPEe", "Harini" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LoginModels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LoginModels",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
