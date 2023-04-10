using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class AuthenticationRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "21f90e07-57eb-400d-a4ff-eadfc40d349d");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Current_Exp", "Email", "EmailConfirmed", "Freemium_Currency", "Level", "LockoutEnabled", "LockoutEnd", "Max_Allowed_Marina_Count", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Premium_Currency", "Role", "Salt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0415e51e-03d3-4850-bca1-cd2561d1a10e", 2, "4fe1e0cc-d7e1-4dbd-9f47-a35c6e1808bf", 0, null, false, 0, 0, false, null, 0, null, null, null, null, false, 0, "", "", "107d0191-f4bb-4a85-bdb4-09154f5d26d9", false, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0415e51e-03d3-4850-bca1-cd2561d1a10e");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Current_Exp", "Email", "EmailConfirmed", "Freemium_Currency", "Level", "LockoutEnabled", "LockoutEnd", "Max_Allowed_Marina_Count", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Premium_Currency", "Role", "Salt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "21f90e07-57eb-400d-a4ff-eadfc40d349d", 2, "3027633a-1c88-4846-840f-3e2b7961e18d", 0, null, false, 0, 0, false, null, 0, null, null, null, null, false, 0, "", "", "b53a15bc-d6a8-4370-bdb6-a7e0c6b565fe", false, null });
        }
    }
}
