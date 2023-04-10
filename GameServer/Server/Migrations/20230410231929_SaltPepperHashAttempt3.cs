using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class SaltPepperHashAttempt3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "09ebccde-c3fb-4fe9-bf86-bb1686e6e7c0");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Current_Exp", "Email", "EmailConfirmed", "Freemium_Currency", "Level", "LockoutEnabled", "LockoutEnd", "Max_Allowed_Marina_Count", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Premium_Currency", "RefreshToken", "RefreshTokenExpiryTime", "Role", "Salt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "57ef5e58-3bca-4cee-b37c-514cc73f8adb", 2, "c46f7d64-9c4f-4207-bb81-da1260f814b0", 0, null, false, 0, 0, false, null, 0, null, null, null, null, false, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "96171e97-cee5-4fae-9765-d033f5987cf6", false, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "57ef5e58-3bca-4cee-b37c-514cc73f8adb");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Current_Exp", "Email", "EmailConfirmed", "Freemium_Currency", "Level", "LockoutEnabled", "LockoutEnd", "Max_Allowed_Marina_Count", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Premium_Currency", "RefreshToken", "RefreshTokenExpiryTime", "Role", "Salt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "09ebccde-c3fb-4fe9-bf86-bb1686e6e7c0", 2, "f6ae87a6-32b5-4f7c-a9f1-4c0b0a8a2653", 0, null, false, 0, 0, false, null, 0, null, null, null, null, false, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "2850bc33-7f08-42c7-9fb8-1424ffecb4b7", false, null });
        }
    }
}
