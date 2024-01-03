using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations.Identity
{
    public partial class CompanySettings_Type_Value : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "company",
                table: "CompanySettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                schema: "company",
                table: "CompanySettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "company",
                table: "CompanySettings");

            migrationBuilder.DropColumn(
                name: "Value",
                schema: "company",
                table: "CompanySettings");
        }
    }
}
