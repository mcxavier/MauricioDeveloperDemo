using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class v0007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardBrandTypeId",
                schema: "payment",
                table: "Payments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CardBrandTypes",
                schema: "payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardBrandTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "payment",
                table: "CardBrandTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Visa" },
                    { 2, "Mastercard" },
                    { 3, "AmericanExpress" },
                    { 4, "Jcb" },
                    { 5, "Discover" },
                    { 6, "Uknown" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardBrandTypeId",
                schema: "payment",
                table: "Payments",
                column: "CardBrandTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CardBrandTypes_CardBrandTypeId",
                schema: "payment",
                table: "Payments",
                column: "CardBrandTypeId",
                principalSchema: "payment",
                principalTable: "CardBrandTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CardBrandTypes_CardBrandTypeId",
                schema: "payment",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "CardBrandTypes",
                schema: "payment");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CardBrandTypeId",
                schema: "payment",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardBrandTypeId",
                schema: "payment",
                table: "Payments");
        }
    }
}
