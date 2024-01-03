using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class v0006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stock_ShopCode",
                schema: "product",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "ShopCode",
                schema: "product",
                table: "Stock");

            migrationBuilder.AddColumn<string>(
                name: "StoreCode",
                schema: "product",
                table: "Stock",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreCode",
                schema: "payment",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreCode",
                schema: "order",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreCode",
                schema: "customer",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreCode",
                schema: "conversation",
                table: "Conversations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreCode",
                schema: "catalog",
                table: "Catalogs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stock_StoreCode",
                schema: "product",
                table: "Stock",
                column: "StoreCode")
                .Annotation("SqlServer:Include", new[] { "StockKeepingUnit" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stock_StoreCode",
                schema: "product",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "StoreCode",
                schema: "product",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "StoreCode",
                schema: "payment",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StoreCode",
                schema: "order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StoreCode",
                schema: "customer",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "StoreCode",
                schema: "conversation",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "StoreCode",
                schema: "catalog",
                table: "Catalogs");

            migrationBuilder.AddColumn<string>(
                name: "ShopCode",
                schema: "product",
                table: "Stock",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ShopCode",
                schema: "product",
                table: "Stock",
                column: "ShopCode")
                .Annotation("SqlServer:Include", new[] { "StockKeepingUnit" });
        }
    }
}
