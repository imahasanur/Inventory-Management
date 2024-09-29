using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagement.Presentation.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminWithClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAtUtc", "Email", "EmailConfirmed", "FirstName", "FullName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAtUtc", "UserName" },
                values: new object[] { new Guid("8dfc0fc9-42cd-4327-b270-50cb36540da6"), 0, "7589169f-2e2d-4e7e-b2d8-3ba2b505ea9c", new DateTime(2024, 9, 29, 11, 54, 2, 430, DateTimeKind.Utc).AddTicks(2383), "admin@gmail.com", true, "Admin", "Admin User", "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEIKibsy6q9XwSJM9d9WAqRzVTjJjcCVKd8MYWJboZ18rV7a/ye+XH5g5XyULf3Zn+w==", null, false, "", false, null, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "role", "admin", new Guid("8dfc0fc9-42cd-4327-b270-50cb36540da6") },
                    { 2, "role", "user", new Guid("8dfc0fc9-42cd-4327-b270-50cb36540da6") }
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
                keyValue: new Guid("8dfc0fc9-42cd-4327-b270-50cb36540da6"));
        }
    }
}
