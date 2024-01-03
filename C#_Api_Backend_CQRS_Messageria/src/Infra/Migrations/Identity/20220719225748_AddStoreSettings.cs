using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations.Identity
{
    public partial class AddStoreSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "OriginId",
            //    schema: "store",
            //    table: "Stores",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "StoreSettingsType",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSettingsType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreSettings",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    StoreId = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreSettings_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "store",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreSettings_StoreSettingsType_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "store",
                        principalTable: "StoreSettingsType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.InsertData(
            //    schema: "company",
            //    table: "CompanySettingsType",
            //    columns: new[] { "Id", "Name" },
            //    values: new object[,]
            //    {
            //        { 4, "LinxCepConfig" },
            //        { 5, "VTexClientConfig" },
            //        { 6, "LinxUxEnvironmentConfig" },
            //        { 7, "PayHubClientConfig" },
            //        { 8, "PagarMeClientConfig" },
            //        { 9, "TwilioConfig" },
            //        { 10, "SmtpConfig" },
            //        { 11, "InstrumentationKeyConfig" }
            //    });

            migrationBuilder.InsertData(
                schema: "store",
                table: "StoreSettingsType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "PagarMeConfig" });

            migrationBuilder.CreateIndex(
                name: "IX_StoreSettings_StoreId",
                schema: "store",
                table: "StoreSettings",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreSettings_TypeId",
                schema: "store",
                table: "StoreSettings",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreSettings",
                schema: "store");

            migrationBuilder.DropTable(
                name: "StoreSettingsType",
                schema: "store");

            //migrationBuilder.DeleteData(
            //    schema: "company",
            //    table: "CompanySettingsType",
            //    keyColumn: "Id",
            //    keyValue: 4);

            //migrationBuilder.DeleteData(
            //    schema: "company",
            //    table: "CompanySettingsType",
            //    keyColumn: "Id",
            //    keyValue: 5);

            //migrationBuilder.DeleteData(
            //    schema: "company",
            //    table: "CompanySettingsType",
            //    keyColumn: "Id",
            //    keyValue: 6);

            //migrationBuilder.DeleteData(
            //    schema: "company",
            //    table: "CompanySettingsType",
            //    keyColumn: "Id",
            //    keyValue: 7);

            //migrationBuilder.DeleteData(
            //    schema: "company",
            //    table: "CompanySettingsType",
            //    keyColumn: "Id",
            //    keyValue: 8);

            //migrationBuilder.DeleteData(
            //    schema: "company",
            //    table: "CompanySettingsType",
            //    keyColumn: "Id",
            //    keyValue: 9);

            //migrationBuilder.DeleteData(
            //    schema: "company",
            //    table: "CompanySettingsType",
            //    keyColumn: "Id",
            //    keyValue: 10);

            //migrationBuilder.DeleteData(
            //    schema: "company",
            //    table: "CompanySettingsType",
            //    keyColumn: "Id",
            //    keyValue: 11);

            migrationBuilder.DropColumn(
                name: "OriginId",
                schema: "store",
                table: "Stores");
        }
    }
}
