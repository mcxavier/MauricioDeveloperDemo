using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations.Identity
{
    public partial class v0002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Tenants_TenantId",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_TenantId",
                schema: "company",
                table: "Companies");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "tenant",
                table: "Tenants",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                schema: "store",
                table: "Stores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                schema: "company",
                table: "Companies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_TenantId",
                schema: "company",
                table: "Companies",
                column: "TenantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Tenants_TenantId",
                schema: "company",
                table: "Companies",
                column: "TenantId",
                principalSchema: "tenant",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Tenants_TenantId",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_TenantId",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                schema: "store",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                schema: "company",
                table: "Companies");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                schema: "tenant",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_Companies_TenantId",
                schema: "company",
                table: "Companies",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Tenants_TenantId",
                schema: "company",
                table: "Companies",
                column: "TenantId",
                principalSchema: "tenant",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
