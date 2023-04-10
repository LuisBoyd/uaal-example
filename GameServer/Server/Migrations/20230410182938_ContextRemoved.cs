using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class ContextRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0415e51e-03d3-4850-bca1-cd2561d1a10e");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Current_Exp", "Email", "EmailConfirmed", "Freemium_Currency", "Level", "LockoutEnabled", "LockoutEnd", "Max_Allowed_Marina_Count", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Premium_Currency", "Role", "Salt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f29406e9-59df-4a88-af4f-c19104dd3b38", 2, "7b5a7fbd-abb5-408e-930d-009a4728a2e1", 0, null, false, 0, 0, false, null, 0, null, null, null, null, false, 0, "", "", "a7ed1478-9074-46e3-8793-e70f3b32b736", false, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f29406e9-59df-4a88-af4f-c19104dd3b38");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Current_Exp", "Email", "EmailConfirmed", "Freemium_Currency", "Level", "LockoutEnabled", "LockoutEnd", "Max_Allowed_Marina_Count", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Premium_Currency", "Role", "Salt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0415e51e-03d3-4850-bca1-cd2561d1a10e", 2, "4fe1e0cc-d7e1-4dbd-9f47-a35c6e1808bf", 0, null, false, 0, 0, false, null, 0, null, null, null, null, false, 0, "", "", "107d0191-f4bb-4a85-bdb4-09154f5d26d9", false, null });
        }
    }
}
