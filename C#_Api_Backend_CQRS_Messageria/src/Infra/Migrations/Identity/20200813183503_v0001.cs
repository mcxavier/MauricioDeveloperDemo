using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations.Identity
{
    public partial class v0001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "company");

            migrationBuilder.EnsureSchema(
                name: "economicGroup");

            migrationBuilder.EnsureSchema(
                name: "store");

            migrationBuilder.EnsureSchema(
                name: "subsidiary");

            migrationBuilder.EnsureSchema(
                name: "tenant");

            migrationBuilder.EnsureSchema(
                name: "user");

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "tenant",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    DataBaseConnectionString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "company",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "tenant",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanySettings",
                schema: "company",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanySettings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EconomicGroups",
                schema: "economicGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CompanyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EconomicGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EconomicGroups_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CompanyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subsidiaries",
                schema: "subsidiary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EconomicGroupId = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subsidiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subsidiaries_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subsidiaries_EconomicGroups_EconomicGroupId",
                        column: x => x.EconomicGroupId,
                        principalSchema: "economicGroup",
                        principalTable: "EconomicGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StoreCode = table.Column<string>(nullable: true),
                    PortalUrl = table.Column<string>(nullable: true),
                    CompanyId = table.Column<Guid>(nullable: true),
                    SubsidiaryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stores_Subsidiaries_SubsidiaryId",
                        column: x => x.SubsidiaryId,
                        principalSchema: "subsidiary",
                        principalTable: "Subsidiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CampaignSettings",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    StoreId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignSettings_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "store",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GatewaySettings",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ApiKey = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    IsSandBox = table.Column<string>(nullable: true),
                    StoreId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatewaySettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GatewaySettings_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "store",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    Port = table.Column<string>(nullable: true),
                    Ssl = table.Column<bool>(nullable: false),
                    UrlOrderSuccessMailTemplate = table.Column<string>(nullable: true),
                    StoreId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Store_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "store",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreAddress",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StoreId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    StreetName = table.Column<string>(nullable: true),
                    StreetNumber = table.Column<string>(nullable: true),
                    Complement = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    DistrictName = table.Column<string>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    StateName = table.Column<string>(nullable: true),
                    CountryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreAddress_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "store",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_TenantId",
                schema: "company",
                table: "Companies",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySettings_CompanyId",
                schema: "company",
                table: "CompanySettings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EconomicGroups_CompanyId",
                schema: "economicGroup",
                table: "EconomicGroups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignSettings_StoreId",
                schema: "store",
                table: "CampaignSettings",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_GatewaySettings_StoreId",
                schema: "store",
                table: "GatewaySettings",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_StoreId",
                schema: "store",
                table: "Store",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAddress_StoreId",
                schema: "store",
                table: "StoreAddress",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CompanyId",
                schema: "store",
                table: "Stores",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_SubsidiaryId",
                schema: "store",
                table: "Stores",
                column: "SubsidiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Subsidiaries_CompanyId",
                schema: "subsidiary",
                table: "Subsidiaries",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Subsidiaries_EconomicGroupId",
                schema: "subsidiary",
                table: "Subsidiaries",
                column: "EconomicGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                schema: "user",
                table: "Users",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanySettings",
                schema: "company");

            migrationBuilder.DropTable(
                name: "CampaignSettings",
                schema: "store");

            migrationBuilder.DropTable(
                name: "GatewaySettings",
                schema: "store");

            migrationBuilder.DropTable(
                name: "Store",
                schema: "store");

            migrationBuilder.DropTable(
                name: "StoreAddress",
                schema: "store");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Stores",
                schema: "store");

            migrationBuilder.DropTable(
                name: "Subsidiaries",
                schema: "subsidiary");

            migrationBuilder.DropTable(
                name: "EconomicGroups",
                schema: "economicGroup");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "company");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "tenant");
        }
    }
}
