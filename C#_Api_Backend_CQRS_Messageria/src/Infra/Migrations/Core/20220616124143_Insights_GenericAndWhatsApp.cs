using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class Insights_GenericAndWhatsApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Received",
                schema: "catalog",
                table: "CatalogCustomer");

            migrationBuilder.EnsureSchema(
                name: "insights");

            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                schema: "catalog",
                table: "CatalogCustomer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InsightDataType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsightDataType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsightData",
                schema: "insights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    StoreCode = table.Column<string>(maxLength: 40, nullable: true),
                    InsightTypeId = table.Column<int>(nullable: false),
                    DecimalValue = table.Column<decimal>(nullable: true),
                    IntValue = table.Column<int>(nullable: true),
                    StringValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsightData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsightData_InsightDataType_InsightTypeId",
                        column: x => x.InsightTypeId,
                        principalTable: "InsightDataType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsightData_InsightTypeId",
                schema: "insights",
                table: "InsightData",
                column: "InsightTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsightData",
                schema: "insights");

            migrationBuilder.DropTable(
                name: "InsightDataType");

            migrationBuilder.DropColumn(
                name: "SentAt",
                schema: "catalog",
                table: "CatalogCustomer");

            migrationBuilder.AddColumn<bool>(
                name: "Received",
                schema: "catalog",
                table: "CatalogCustomer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
