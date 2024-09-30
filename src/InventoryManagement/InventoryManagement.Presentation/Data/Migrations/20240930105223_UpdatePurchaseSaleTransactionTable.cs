using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Presentation.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePurchaseSaleTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "SaleOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "PurchaseOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d88d9d62-162d-4044-be26-fc89c796b4e7"),
                columns: new[] { "ConcurrencyStamp", "CreatedAtUtc", "PasswordHash" },
                values: new object[] { "642e5b89-30ae-4954-8923-60944cea9e3d", new DateTime(2024, 9, 30, 10, 52, 22, 902, DateTimeKind.Utc).AddTicks(6010), "AQAAAAIAAYagAAAAEFIWgdIAHBwT8qcoLr7113dOFwNmnm8UBPYA3Q/x/2YnRQUlRyaTZOlBq4TlLKFRXQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "User",
                table: "SaleOrder");

            migrationBuilder.DropColumn(
                name: "User",
                table: "PurchaseOrder");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d88d9d62-162d-4044-be26-fc89c796b4e7"),
                columns: new[] { "ConcurrencyStamp", "CreatedAtUtc", "PasswordHash" },
                values: new object[] { "ce20fba1-c3e2-4ffb-9fbe-f6a2c0e4d9b1", new DateTime(2024, 9, 30, 10, 47, 48, 155, DateTimeKind.Utc).AddTicks(4238), "AQAAAAIAAYagAAAAEKfGXP7WxPNzbQ042nDZIU5KcZZ6W8x7hQshpkioc/RqCxb42//r1KykSli5UVX1jA==" });
        }
    }
}
