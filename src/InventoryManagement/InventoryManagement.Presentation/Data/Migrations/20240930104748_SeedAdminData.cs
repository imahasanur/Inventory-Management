using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagement.Presentation.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAtUtc", "Email", "EmailConfirmed", "FirstName", "FullName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAtUtc", "UserName" },
                values: new object[] { new Guid("d88d9d62-162d-4044-be26-fc89c796b4e7"), 0, "ce20fba1-c3e2-4ffb-9fbe-f6a2c0e4d9b1", new DateTime(2024, 9, 30, 10, 47, 48, 155, DateTimeKind.Utc).AddTicks(4238), "admin@gmail.com", true, "Admin", "Admin User", "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEKfGXP7WxPNzbQ042nDZIU5KcZZ6W8x7hQshpkioc/RqCxb42//r1KykSli5UVX1jA==", null, false, "", false, null, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "role", "admin", new Guid("d88d9d62-162d-4044-be26-fc89c796b4e7") },
                    { 2, "role", "user", new Guid("d88d9d62-162d-4044-be26-fc89c796b4e7") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d88d9d62-162d-4044-be26-fc89c796b4e7"));
        }
    }
}
