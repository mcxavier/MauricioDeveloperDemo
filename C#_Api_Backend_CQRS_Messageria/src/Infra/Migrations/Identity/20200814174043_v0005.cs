using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations.Identity
{
    public partial class v0005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsSandBox",
                schema: "store",
                table: "GatewaySettings",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IsSandBox",
                schema: "store",
                table: "GatewaySettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
