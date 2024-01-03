using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class OrderStatus_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Document",
                schema: "customer",
                table: "Sellers",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Pedido pago");

            migrationBuilder.UpdateData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Separação em estoque");

            migrationBuilder.UpdateData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Pedido Confirmado");

            migrationBuilder.InsertData(
                schema: "order",
                table: "Status",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 9, "Falha na Integração" },
                    { 10, "Pedido Integrado" },
                    { 11, "Pedido finalizado" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DropColumn(
                name: "Document",
                schema: "customer",
                table: "Sellers");

            migrationBuilder.UpdateData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Separação em estoque");

            migrationBuilder.UpdateData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Pedido pago");

            migrationBuilder.UpdateData(
                schema: "order",
                table: "Status",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Pedido finalizado");
        }
    }
}
