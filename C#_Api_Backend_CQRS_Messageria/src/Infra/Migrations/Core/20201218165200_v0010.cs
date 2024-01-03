using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class v0010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "marketing");

            migrationBuilder.CreateTable(
                name: "CustomerNotification",
                schema: "marketing",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StoreCode = table.Column<string>(maxLength: 25, nullable: true),
                    StockKeepingUnit = table.Column<string>(maxLength: 40, nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerEmail = table.Column<string>(nullable: true),
                    Notified = table.Column<bool>(nullable: false),
                    NotifiedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerNotification", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotification_StoreCode",
                schema: "marketing",
                table: "CustomerNotification",
                column: "StoreCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerNotification",
                schema: "marketing");
        }
    }
}
