using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class v0001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.EnsureSchema(
                name: "customer");

            migrationBuilder.EnsureSchema(
                name: "geography");

            migrationBuilder.EnsureSchema(
                name: "conversation");

            migrationBuilder.EnsureSchema(
                name: "order");

            migrationBuilder.EnsureSchema(
                name: "payment");

            migrationBuilder.EnsureSchema(
                name: "product");

            migrationBuilder.CreateTable(
                name: "Catalogs",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: true),
                    Rg = table.Column<string>(nullable: true),
                    BirthDay = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                schema: "customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    BirthDay = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "geography",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZipCode = table.Column<string>(maxLength: 8, nullable: true),
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
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemDiscountTypes",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDiscountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderType",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatus",
                schema: "payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymenTypes",
                schema: "payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymenTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplierCodeTypes",
                schema: "payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierCodeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionStatus",
                schema: "payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BrandName = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    CommonReference = table.Column<string>(nullable: true),
                    Ncm = table.Column<string>(nullable: true),
                    OriginId = table.Column<string>(nullable: true),
                    Origin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecificationTypes",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogCustomer",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogCustomer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogCustomer_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalSchema: "catalog",
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogCustomer_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "customer",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                schema: "conversation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    IntegrationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "customer",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pan = table.Column<string>(nullable: true),
                    Amount = table.Column<long>(nullable: false),
                    Installments = table.Column<long>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    GatewayProvider = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    PaymentTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_PaymenTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalSchema: "payment",
                        principalTable: "PaymenTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentStatus_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "payment",
                        principalTable: "PaymentStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogProduct",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    CatalogId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogProduct_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalSchema: "catalog",
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogProduct_Catalogs_CatalogId1",
                        column: x => x.CatalogId1,
                        principalSchema: "catalog",
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CatalogProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "product",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "product",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategory_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "product",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Variations",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    StockKeepingUnit = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CompleteName = table.Column<string>(nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ListPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    OriginId = table.Column<string>(nullable: true),
                    Origin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Variations_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "product",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "conversation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(nullable: true),
                    SendAt = table.Column<DateTime>(nullable: true),
                    ConversationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalSchema: "conversation",
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Suplier = table.Column<string>(nullable: true),
                    Installments = table.Column<int>(nullable: false),
                    Ammount = table.Column<int>(nullable: false),
                    IsSuccess = table.Column<bool>(nullable: false),
                    Nsu = table.Column<string>(nullable: true),
                    AuthorizationCode = table.Column<long>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    SuplierReturnCode = table.Column<int>(nullable: true),
                    SuplierReturnId = table.Column<int>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    PaymentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "payment",
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionStatus_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "payment",
                        principalTable: "TransactionStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_SupplierCodeTypes_SuplierReturnId",
                        column: x => x.SuplierReturnId,
                        principalSchema: "payment",
                        principalTable: "SupplierCodeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    UrlImage = table.Column<string>(nullable: true),
                    IsPrincipal = table.Column<bool>(nullable: false),
                    ProductVariationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Variations_ProductVariationId",
                        column: x => x.ProductVariationId,
                        principalSchema: "product",
                        principalTable: "Variations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Specifications",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 25, nullable: true),
                    Value = table.Column<string>(maxLength: 25, nullable: true),
                    IsFilter = table.Column<bool>(nullable: false),
                    ProductVariationId = table.Column<int>(nullable: true),
                    TypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specifications_Variations_ProductVariationId",
                        column: x => x.ProductVariationId,
                        principalSchema: "product",
                        principalTable: "Variations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Specifications_SpecificationTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "product",
                        principalTable: "SpecificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OrderedAt = table.Column<DateTime>(nullable: false),
                    ShippingAmmount = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: true),
                    Value = table.Column<decimal>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true),
                    SellerId = table.Column<int>(nullable: true),
                    CatalogId = table.Column<int>(nullable: true),
                    ShippingId = table.Column<int>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    OrderTypeId = table.Column<int>(nullable: false),
                    PaymentId = table.Column<int>(nullable: true),
                    SendedToUx = table.Column<bool>(nullable: false),
                    CustomerId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalSchema: "catalog",
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "customer",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId1",
                        column: x => x.CustomerId1,
                        principalSchema: "customer",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderType_OrderTypeId",
                        column: x => x.OrderTypeId,
                        principalSchema: "order",
                        principalTable: "OrderType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "payment",
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalSchema: "customer",
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Status_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "order",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StockKeepingUnit = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    UrlPicture = table.Column<string>(nullable: true),
                    Units = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitDiscount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductVariationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "order",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "product",
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Variations_ProductVariationId",
                        column: x => x.ProductVariationId,
                        principalSchema: "product",
                        principalTable: "Variations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Shippings",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shippings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shippings_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "geography",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shippings_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "order",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemDiscounts",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(nullable: false),
                    ReferenceId = table.Column<int>(nullable: false),
                    DiscountTypeId = table.Column<int>(nullable: false),
                    OrderItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDiscounts_ItemDiscountTypes_DiscountTypeId",
                        column: x => x.DiscountTypeId,
                        principalSchema: "order",
                        principalTable: "ItemDiscountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemDiscounts_Items_OrderItemId",
                        column: x => x.OrderItemId,
                        principalSchema: "order",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "order",
                table: "ItemDiscountTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Campaign" },
                    { 2, "Discount" }
                });

            migrationBuilder.InsertData(
                schema: "order",
                table: "OrderType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Shipment" },
                    { 2, "Pickup" }
                });

            migrationBuilder.InsertData(
                schema: "order",
                table: "Status",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pedido realizado" },
                    { 2, "Aguardando validação" },
                    { 3, "Separação em estoque" },
                    { 4, "Pedido pago" },
                    { 5, "Saída para entrega" },
                    { 6, "Cancelado" }
                });

            migrationBuilder.InsertData(
                schema: "payment",
                table: "PaymenTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Cartão de crédito" },
                    { 2, "Cartão de Débito" },
                    { 3, "Boleto" }
                });

            migrationBuilder.InsertData(
                schema: "payment",
                table: "PaymentStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "Processed" },
                    { 1, "Processing" },
                    { 3, "Failed" }
                });

            migrationBuilder.InsertData(
                schema: "payment",
                table: "SupplierCodeTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 9, "Problemas no Cartão. O portador do cartão deve entrar em contato com o emissor do cartão.", "CardProblems" },
                    { 20, "Transação não autorizada. Entre em contato com nosso suporte.", "NotAuthorizedCallSupport" },
                    { 19, "Transação enviada anteriormente", "TransactionSendBefore" },
                    { 18, "Erro no Processamento. Tente novamente.", "ProcessingError" },
                    { 17, "Número do Cartão Inválido", "InvalidCardNumber" },
                    { 16, "Não Autorizado. Código de Segurança Inválido.", "NotAuthorizedInvalidSecurityNumber" },
                    { 14, "Transação não encontrada.", "TransactionNotFound" },
                    { 13, "Não Autorizado. Risco identificado pelo emissor.", "NotAuthorizedIdentifiedRisk" },
                    { 12, "Transação não autorizada.", "NotAuthorizedNoReason" },
                    { 11, "Não Autorizado. Cartão não existente.", "NotAuthorizedNonExistentCard" },
                    { 10, "Não Autorizado. Tente novamente.", "NotAuthorizedTryAgain" },
                    { 8, "Cartão Sem Limite. O portador do cartão deve entrar em contato com o emissor do cartão.", "CardLimitExceeded" },
                    { 3, "Problemas no credenciamento. Entre em contato com nosso suporte.", "ErrorOnCredentials" },
                    { 6, "Falha de Comunicação. Tente novamente.", "ComunicationFailure" },
                    { 5, "Transação não permitida. Entre em contato com nosso suporte.", "TransactionNotAllowed" },
                    { 4, "Não Autorizado. O portador do cartão deve entrar em contato com o emissor do cartão.", "NotAuthorized" },
                    { 2, "Erro nos dados reportados.", "ErrorOnDataReported" },
                    { 1, "Transação não permitida para o emissor.", "NotAllowed" },
                    { 7, "Cartão Expirado. O portador do cartão deve entrar em contato com o emissor do cartão.", "ExpiredCard" }
                });

            migrationBuilder.InsertData(
                schema: "payment",
                table: "TransactionStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Authorized" },
                    { 2, "PreAuthorized" },
                    { 3, "Captured" },
                    { 4, "Canceled" },
                    { 6, "Reject" },
                    { 7, "UnderInvestigation" },
                    { 8, "Unknown" },
                    { 9, "Release" },
                    { 5, "NotAuthorized" }
                });

            migrationBuilder.InsertData(
                schema: "product",
                table: "SpecificationTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "SimpleColor" },
                    { 1, "Size" },
                    { 2, "Color" },
                    { 3, "Gender" },
                    { 50, "Others" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogCustomer_CatalogId",
                schema: "catalog",
                table: "CatalogCustomer",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogCustomer_CustomerId",
                schema: "catalog",
                table: "CatalogCustomer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProduct_CatalogId",
                schema: "catalog",
                table: "CatalogProduct",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProduct_CatalogId1",
                schema: "catalog",
                table: "CatalogProduct",
                column: "CatalogId1");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProduct_ProductId",
                schema: "catalog",
                table: "CatalogProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_CustomerId",
                schema: "conversation",
                table: "Conversations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                schema: "conversation",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDiscounts_DiscountTypeId",
                schema: "order",
                table: "ItemDiscounts",
                column: "DiscountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDiscounts_OrderItemId",
                schema: "order",
                table: "ItemDiscounts",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_OrderId",
                schema: "order",
                table: "Items",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ProductId",
                schema: "order",
                table: "Items",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ProductVariationId",
                schema: "order",
                table: "Items",
                column: "ProductVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CatalogId",
                schema: "order",
                table: "Orders",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                schema: "order",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId1",
                schema: "order",
                table: "Orders",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTypeId",
                schema: "order",
                table: "Orders",
                column: "OrderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentId",
                schema: "order",
                table: "Orders",
                column: "PaymentId",
                unique: true,
                filter: "[PaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerId",
                schema: "order",
                table: "Orders",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingId",
                schema: "order",
                table: "Orders",
                column: "ShippingId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId",
                schema: "order",
                table: "Orders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Shippings_AddressId",
                schema: "order",
                table: "Shippings",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Shippings_OrderId",
                schema: "order",
                table: "Shippings",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentTypeId",
                schema: "payment",
                table: "Payments",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StatusId",
                schema: "payment",
                table: "Payments",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentId",
                schema: "payment",
                table: "Transactions",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StatusId",
                schema: "payment",
                table: "Transactions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SuplierReturnId",
                schema: "payment",
                table: "Transactions",
                column: "SuplierReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductVariationId",
                schema: "product",
                table: "Images",
                column: "ProductVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_CategoryId",
                schema: "product",
                table: "ProductCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_ProductId",
                schema: "product",
                table: "ProductCategory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_Description",
                schema: "product",
                table: "Specifications",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_ProductVariationId",
                schema: "product",
                table: "Specifications",
                column: "ProductVariationId")
                .Annotation("SqlServer:Include", new[] { "TypeId", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_TypeId",
                schema: "product",
                table: "Specifications",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_Value",
                schema: "product",
                table: "Specifications",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_Variations_BasePrice",
                schema: "product",
                table: "Variations",
                column: "BasePrice");

            migrationBuilder.CreateIndex(
                name: "IX_Variations_CostPrice",
                schema: "product",
                table: "Variations",
                column: "CostPrice");

            migrationBuilder.CreateIndex(
                name: "IX_Variations_ListPrice",
                schema: "product",
                table: "Variations",
                column: "ListPrice");

            migrationBuilder.CreateIndex(
                name: "IX_Variations_ProductId",
                schema: "product",
                table: "Variations",
                column: "ProductId")
                .Annotation("SqlServer:Include", new[] { "BasePrice", "IsActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shippings_ShippingId",
                schema: "order",
                table: "Orders",
                column: "ShippingId",
                principalSchema: "order",
                principalTable: "Shippings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Catalogs_CatalogId",
                schema: "order",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                schema: "order",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId1",
                schema: "order",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_Orders_OrderId",
                schema: "order",
                table: "Shippings");

            migrationBuilder.DropTable(
                name: "CatalogCustomer",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "CatalogProduct",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "conversation");

            migrationBuilder.DropTable(
                name: "ItemDiscounts",
                schema: "order");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "Images",
                schema: "product");

            migrationBuilder.DropTable(
                name: "ProductCategory",
                schema: "product");

            migrationBuilder.DropTable(
                name: "Specifications",
                schema: "product");

            migrationBuilder.DropTable(
                name: "Conversations",
                schema: "conversation");

            migrationBuilder.DropTable(
                name: "ItemDiscountTypes",
                schema: "order");

            migrationBuilder.DropTable(
                name: "Items",
                schema: "order");

            migrationBuilder.DropTable(
                name: "TransactionStatus",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "SupplierCodeTypes",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "product");

            migrationBuilder.DropTable(
                name: "SpecificationTypes",
                schema: "product");

            migrationBuilder.DropTable(
                name: "Variations",
                schema: "product");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "product");

            migrationBuilder.DropTable(
                name: "Catalogs",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "customer");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "order");

            migrationBuilder.DropTable(
                name: "OrderType",
                schema: "order");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "Sellers",
                schema: "customer");

            migrationBuilder.DropTable(
                name: "Shippings",
                schema: "order");

            migrationBuilder.DropTable(
                name: "Status",
                schema: "order");

            migrationBuilder.DropTable(
                name: "PaymenTypes",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "PaymentStatus",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "geography");
        }
    }
}
