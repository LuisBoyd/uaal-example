using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class NoSaltAndRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9a947593-608f-4758-b00f-b28aed5be4d9");

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Current_Exp", "Email", "EmailConfirmed", "Freemium_Currency", "Level", "LockoutEnabled", "LockoutEnd", "Max_Allowed_Marina_Count", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Premium_Currency", "RefreshToken", "RefreshTokenExpiryTime", "Role", "Salt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "cc7a5ba9-85d0-4a80-b004-3bdabf2c608c", 2, "ba61d8d4-b925-40b8-9f38-5656116aa5bf", 0, null, false, 0, 0, false, null, 0, null, null, null, null, false, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "c77bad83-c0d4-42ec-8e27-be2b91447182", false, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cc7a5ba9-85d0-4a80-b004-3bdabf2c608c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Salt",
                keyValue: null,
                column: "Salt",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Role",
                keyValue: null,
                column: "Role",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Current_Exp", "Email", "EmailConfirmed", "Freemium_Currency", "Level", "LockoutEnabled", "LockoutEnd", "Max_Allowed_Marina_Count", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Premium_Currency", "RefreshToken", "RefreshTokenExpiryTime", "Role", "Salt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "9a947593-608f-4758-b00f-b28aed5be4d9", 2, "77311881-2eab-4bfd-beda-891e9dab0e0b", 0, null, false, 0, 0, false, null, 0, null, null, null, null, false, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "996295c1-0554-4f59-b2cf-27b34bfd84b8", false, null });
        }
    }
}
