using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class FixesHomologacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "order",
                table: "Status",
                columns: new[] { "Id", "Name" },
                values: new object[] { 12, "Pedido faturado" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
