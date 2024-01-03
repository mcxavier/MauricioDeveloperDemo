using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class v0009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StockKeepingUnit",
                schema: "product",
                table: "Variations",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StockKeepingUnit",
                schema: "customer",
                table: "OrderHistory",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Variations_StockKeepingUnit",
                schema: "product",
                table: "Variations",
                column: "StockKeepingUnit");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_StockKeepingUnit",
                schema: "customer",
                table: "OrderHistory",
                column: "StockKeepingUnit");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHistory_Variations_StockKeepingUnit",
                schema: "customer",
                table: "OrderHistory",
                column: "StockKeepingUnit",
                principalSchema: "product",
                principalTable: "Variations",
                principalColumn: "StockKeepingUnit",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHistory_Variations_StockKeepingUnit",
                schema: "customer",
                table: "OrderHistory");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Variations_StockKeepingUnit",
                schema: "product",
                table: "Variations");

            migrationBuilder.DropIndex(
                name: "IX_OrderHistory_StockKeepingUnit",
                schema: "customer",
                table: "OrderHistory");

            migrationBuilder.AlterColumn<string>(
                name: "StockKeepingUnit",
                schema: "product",
                table: "Variations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "StockKeepingUnit",
                schema: "customer",
                table: "OrderHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
