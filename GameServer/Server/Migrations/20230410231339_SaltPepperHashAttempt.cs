using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class SaltPepperHashAttempt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cc7a5ba9-85d0-4a80-b004-3bdabf2c608c");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Current_Exp", "Email", "EmailConfirmed", "Freemium_Currency", "Level", "LockoutEnabled", "LockoutEnd", "Max_Allowed_Marina_Count", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Premium_Currency", "RefreshToken", "RefreshTokenExpiryTime", "Role", "Salt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "98e2be65-fc22-4d64-bb17-2add0b1baa33", 2, "f9bc2584-d281-445d-93a3-6fbca4cf3393", 0, null, false, 0, 0, false, null, 0, null, null, null, null, false, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "b23b213d-7da8-4212-b2cf-53dd19a29760", false, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98e2be65-fc22-4d64-bb17-2add0b1baa33");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Current_Exp", "Email", "EmailConfirmed", "Freemium_Currency", "Level", "LockoutEnabled", "LockoutEnd", "Max_Allowed_Marina_Count", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Premium_Currency", "RefreshToken", "RefreshTokenExpiryTime", "Role", "Salt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "cc7a5ba9-85d0-4a80-b004-3bdabf2c608c", 2, "ba61d8d4-b925-40b8-9f38-5656116aa5bf", 0, null, false, 0, 0, false, null, 0, null, null, null, null, false, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "c77bad83-c0d4-42ec-8e27-be2b91447182", false, null });
        }
    }
}
