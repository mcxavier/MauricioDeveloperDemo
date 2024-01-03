using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class v0008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderHistory",
                schema: "customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    StoreCode = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: true),
                    StockKeepingUnit = table.Column<string>(nullable: true),
                    GrossValue = table.Column<decimal>(nullable: true),
                    Discount = table.Column<decimal>(nullable: true),
                    NetValue = table.Column<decimal>(nullable: true),
                    Units = table.Column<int>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: true),
                    SellerName = table.Column<string>(nullable: true),
                    OrderOrigin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHistory_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "customer",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_CustomerId",
                schema: "customer",
                table: "OrderHistory",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderHistory",
                schema: "customer");
        }
    }
}
