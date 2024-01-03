using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations.Identity
{
    public partial class v0004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceUrl",
                schema: "store",
                table: "CampaignSettings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ErpSettings",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    AppHost = table.Column<string>(nullable: true),
                    ServiceHost = table.Column<string>(nullable: true),
                    Environment = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    StoreId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErpSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErpSettings_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "store",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ErpSettings_StoreId",
                schema: "store",
                table: "ErpSettings",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErpSettings",
                schema: "store");

            migrationBuilder.DropColumn(
                name: "ServiceUrl",
                schema: "store",
                table: "CampaignSettings");
        }
    }
}
