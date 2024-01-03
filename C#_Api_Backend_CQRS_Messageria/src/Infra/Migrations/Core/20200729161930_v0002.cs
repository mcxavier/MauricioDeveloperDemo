using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class v0002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stock",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopCode = table.Column<string>(maxLength: 25, nullable: true),
                    StockKeepingUnit = table.Column<string>(maxLength: 40, nullable: true),
                    LastSinc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ShopCode",
                schema: "product",
                table: "Stock",
                column: "ShopCode")
                .Annotation("SqlServer:Include", new[] { "StockKeepingUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_Stock_StockKeepingUnit",
                schema: "product",
                table: "Stock",
                column: "StockKeepingUnit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stock",
                schema: "product");
        }
    }
}
