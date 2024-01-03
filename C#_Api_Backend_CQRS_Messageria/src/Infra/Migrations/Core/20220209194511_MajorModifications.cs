using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class MajorModifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderCode",
                schema: "order",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BeginsAt",
                schema: "catalog",
                table: "Catalogs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                schema: "catalog",
                table: "Catalogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumOfPieces",
                schema: "catalog",
                table: "Catalogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumOfSales",
                schema: "catalog",
                table: "Catalogs",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Revenues",
                schema: "catalog",
                table: "Catalogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                schema: "catalog",
                table: "Catalogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SentContacts",
                schema: "catalog",
                table: "Catalogs",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Received",
                schema: "catalog",
                table: "CatalogCustomer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                schema: "order",
                table: "Status",
                columns: new[] { "Id", "Name" },
                values: new object[] { 7, "Falha no pagamento" });

            migrationBuilder.InsertData(
                schema: "order",
                table: "Status",
                columns: new[] { "Id", "Name" },
                values: new object[] { 8, "Pedido finalizado" });

            migrationBuilder.InsertData(
                schema: "payment",
                table: "PaymenTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Pix" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "payment",
                table: "PaymenTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "OrderCode",
                schema: "order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BeginsAt",
                schema: "catalog",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                schema: "catalog",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "NumOfPieces",
                schema: "catalog",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "NumOfSales",
                schema: "catalog",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "Revenues",
                schema: "catalog",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "SellerId",
                schema: "catalog",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "SentContacts",
                schema: "catalog",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "Received",
                schema: "catalog",
                table: "CatalogCustomer");
        }
    }
}
