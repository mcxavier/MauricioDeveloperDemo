using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class v0004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Variations_CostPrice",
                schema: "product",
                table: "Variations");

            migrationBuilder.DropIndex(
                name: "IX_Variations_ListPrice",
                schema: "product",
                table: "Variations");

            migrationBuilder.DropIndex(
                name: "IX_Stock_StockKeepingUnit",
                schema: "product",
                table: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_TypeId",
                schema: "product",
                table: "Specifications");

            migrationBuilder.CreateIndex(
                name: "IX_Variations_IsActive",
                schema: "product",
                table: "Variations",
                column: "IsActive")
                .Annotation("SqlServer:Include", new[] { "StockKeepingUnit", "BasePrice", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Stock_StockKeepingUnit",
                schema: "product",
                table: "Stock",
                column: "StockKeepingUnit")
                .Annotation("SqlServer:Include", new[] { "Units" });

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_TypeId",
                schema: "product",
                table: "Specifications",
                column: "TypeId")
                .Annotation("SqlServer:Include", new[] { "ProductVariationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Variations_IsActive",
                schema: "product",
                table: "Variations");

            migrationBuilder.DropIndex(
                name: "IX_Stock_StockKeepingUnit",
                schema: "product",
                table: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_TypeId",
                schema: "product",
                table: "Specifications");

            migrationBuilder.CreateIndex(
                name: "IX_Variations_CostPrice",
                schema: "product",
                table: "Variations",
                column: "CostPrice");

            migrationBuilder.CreateIndex(
                name: "IX_Variations_ListPrice",
                schema: "product",
                table: "Variations",
                column: "ListPrice");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_StockKeepingUnit",
                schema: "product",
                table: "Stock",
                column: "StockKeepingUnit");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_TypeId",
                schema: "product",
                table: "Specifications",
                column: "TypeId");
        }
    }
}
