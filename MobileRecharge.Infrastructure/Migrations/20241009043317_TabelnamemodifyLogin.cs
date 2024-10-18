using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopUpAPI.Migrations
{
    /// <inheritdoc />
    public partial class TabelnamemodifyLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginModels",
                table: "LoginModels");

            migrationBuilder.RenameTable(
                name: "LoginModels",
                newName: "UserLogin");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogin",
                table: "UserLogin",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "UserLogin",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$S6gF2hkeXnxB9U1qKbIu/.2dbhnVJs.7fKy8frGY89qUrNVJ.j9z.");

            migrationBuilder.UpdateData(
                table: "UserLogin",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$0Y2jKEAHz7269z2MIm3f9.kYMj3mSN9FBhGwwLdsOKevpmWrVMXwO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogin",
                table: "UserLogin");

            migrationBuilder.RenameTable(
                name: "UserLogin",
                newName: "LoginModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginModels",
                table: "LoginModels",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "LoginModels",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$uOEdhHs.vGGfyyTBXhxS3uW10.GU4cN7ax43TXYsz62AGxM5qxjPe");

            migrationBuilder.UpdateData(
                table: "LoginModels",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$5OrC0nurkwVhqjpwsIxYi.wqxPktD3sDuxgcGYhCvcMFIAvnHvPEe");
        }
    }
}
