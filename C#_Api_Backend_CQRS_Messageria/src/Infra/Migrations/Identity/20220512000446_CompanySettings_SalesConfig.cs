using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations.Identity
{
    public partial class CompanySettings_SalesConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "company",
                table: "CompanySettings");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                schema: "company",
                table: "CompanySettings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanySettingsType",
                schema: "company",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySettingsType", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "company",
                table: "CompanySettingsType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "LinxIOConfig" });

            migrationBuilder.InsertData(
                schema: "company",
                table: "CompanySettingsType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "ReshopConfig" });

            migrationBuilder.InsertData(
                schema: "company",
                table: "CompanySettingsType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "SalesConfig" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanySettings_TypeId",
                schema: "company",
                table: "CompanySettings",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySettings_CompanySettingsType_TypeId",
                schema: "company",
                table: "CompanySettings",
                column: "TypeId",
                principalSchema: "company",
                principalTable: "CompanySettingsType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanySettings_CompanySettingsType_TypeId",
                schema: "company",
                table: "CompanySettings");

            migrationBuilder.DropTable(
                name: "CompanySettingsType",
                schema: "company");

            migrationBuilder.DropIndex(
                name: "IX_CompanySettings_TypeId",
                schema: "company",
                table: "CompanySettings");

            migrationBuilder.DropColumn(
                name: "TypeId",
                schema: "company",
                table: "CompanySettings");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "company",
                table: "CompanySettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
