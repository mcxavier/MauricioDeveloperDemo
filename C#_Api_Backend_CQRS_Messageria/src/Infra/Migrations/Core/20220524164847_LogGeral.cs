using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class LogGeral : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "log");

            migrationBuilder.CreateTable(
                name: "LogGeral",
                schema: "log",
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
                    TopicId = table.Column<string>(maxLength: 60, nullable: true),
                    EntityId = table.Column<string>(maxLength: 30, nullable: true),
                    ReferenceMessageId = table.Column<string>(maxLength: 120, nullable: true),
                    Type = table.Column<string>(maxLength: 20, nullable: true),
                    Message = table.Column<string>(maxLength: 200, nullable: true),
                    MessageJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogGeral", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogGeral",
                schema: "log");
        }
    }
}
