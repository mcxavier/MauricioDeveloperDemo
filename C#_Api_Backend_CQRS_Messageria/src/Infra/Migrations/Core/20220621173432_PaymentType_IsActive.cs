using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class PaymentType_IsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "payment",
                table: "PaymenTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "payment",
                table: "PaymenTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                schema: "payment",
                table: "PaymenTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsActive",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "payment",
                table: "PaymenTypes");
        }
    }
}
