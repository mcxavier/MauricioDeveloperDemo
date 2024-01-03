using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class StockUniqueBySKUAndStoreCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StoreCode",
                schema: "product",
                table: "Stock",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StockKeepingUnit",
                schema: "product",
                table: "Stock",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "UniqueStockRegister",
                schema: "product",
                table: "Stock",
                columns: new[] { "StockKeepingUnit", "StoreCode" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "UniqueStockRegister",
                schema: "product",
                table: "Stock");

            migrationBuilder.AlterColumn<string>(
                name: "StoreCode",
                schema: "product",
                table: "Stock",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "StockKeepingUnit",
                schema: "product",
                table: "Stock",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 40);
        }
    }
}
