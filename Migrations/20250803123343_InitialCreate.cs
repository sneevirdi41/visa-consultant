using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace visa_consulatant.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactInquiries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    VisaType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInquiries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomePageContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    HeroImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    BannerImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    WelcomeMessage = table.Column<string>(type: "text", nullable: true),
                    ServicesOverview = table.Column<string>(type: "text", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    ImageData = table.Column<byte[]>(type: "bytea", nullable: false),
                    ImageType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UploadedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageUploads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisaServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ProcessingTime = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisaServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarouselImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ImageUploadId = table.Column<int>(type: "integer", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarouselImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarouselImages_ImageUploads_ImageUploadId",
                        column: x => x.ImageUploadId,
                        principalTable: "ImageUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImageUploadId = table.Column<int>(type: "integer", nullable: true),
                    DiscountPercentage = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_ImageUploads_ImageUploadId",
                        column: x => x.ImageUploadId,
                        principalTable: "ImageUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "HomePageContents",
                columns: new[] { "Id", "BannerImageUrl", "Description", "HeroImageUrl", "LastUpdated", "LogoUrl", "ServicesOverview", "Title", "UpdatedBy", "WelcomeMessage" },
                values: new object[] { 1, null, "Your trusted partner for all Australian immigration services. We provide expert guidance and support for your visa applications to Australia.", null, new DateTime(2025, 8, 3, 12, 33, 41, 22, DateTimeKind.Utc).AddTicks(2375), null, "We offer comprehensive Australian immigration services including skilled migration, student visas, partner visas, tourist visas, and permanent residency applications.", "Guru Kirpa Immigration - Your Gateway to Australia", "System", "Welcome to Guru Kirpa Immigration - Your Gateway to Australia" });

            migrationBuilder.InsertData(
                table: "Offers",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "DiscountPercentage", "ImageUploadId", "IsActive", "Title", "ValidFrom", "ValidUntil" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 3, 12, 33, 41, 24, DateTimeKind.Utc).AddTicks(8384), "System", "Get 20% off on student visa processing fees. Limited time offer for new students!", 20.00m, null, true, "Student Visa Special Offer", new DateTime(2025, 8, 3, 12, 33, 41, 24, DateTimeKind.Utc).AddTicks(4601), new DateTime(2025, 11, 3, 12, 33, 41, 24, DateTimeKind.Utc).AddTicks(5973) },
                    { 2, new DateTime(2025, 8, 3, 12, 33, 41, 25, DateTimeKind.Utc).AddTicks(554), "System", "Complete family immigration package with special rates for families of 3 or more members.", 15.00m, null, true, "Family Package Deal", new DateTime(2025, 8, 3, 12, 33, 41, 25, DateTimeKind.Utc).AddTicks(527), new DateTime(2026, 2, 3, 12, 33, 41, 25, DateTimeKind.Utc).AddTicks(529) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, new DateTime(2025, 8, 3, 12, 33, 41, 16, DateTimeKind.Utc).AddTicks(8001), "admin@gurukirpa.com", true, "$2a$11$am8zvlm8es8A0tsF70NdPuysIEZ.EDyhqN6i8A3Rl4Pmqu6PqSTmS", "Admin", "admin" });

            migrationBuilder.InsertData(
                table: "VisaServices",
                columns: new[] { "Id", "Country", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Price", "ProcessingTime", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Australia", new DateTime(2025, 8, 3, 12, 33, 41, 23, DateTimeKind.Utc).AddTicks(4420), "Professional skilled migration services for qualified workers seeking permanent residency in Australia through points-based system.", null, true, "Skilled Migration Visa", null, "6-12 months", null },
                    { 2, "Australia", new DateTime(2025, 8, 3, 12, 33, 41, 23, DateTimeKind.Utc).AddTicks(5567), "Comprehensive student visa services for international students wishing to study at Australian educational institutions.", null, true, "Student Visa (Subclass 500)", null, "1-3 months", null },
                    { 3, "Australia", new DateTime(2025, 8, 3, 12, 33, 41, 23, DateTimeKind.Utc).AddTicks(5572), "Expert guidance for partner visa applications including spouse, de facto, and prospective marriage visas.", null, true, "Partner Visa", null, "12-24 months", null },
                    { 4, "Australia", new DateTime(2025, 8, 3, 12, 33, 41, 23, DateTimeKind.Utc).AddTicks(5577), "Tourist and visitor visa services for short-term stays in Australia for tourism, business, or family visits.", null, true, "Tourist Visa (Subclass 600)", null, "2-4 weeks", null },
                    { 5, "Australia", new DateTime(2025, 8, 3, 12, 33, 41, 23, DateTimeKind.Utc).AddTicks(5582), "Temporary skill shortage visa services for skilled workers sponsored by Australian employers.", null, true, "Work Visa (Subclass 482)", null, "3-6 months", null },
                    { 6, "Australia", new DateTime(2025, 8, 3, 12, 33, 41, 23, DateTimeKind.Utc).AddTicks(5586), "Business and investment visa services for entrepreneurs and investors seeking to establish businesses in Australia.", null, true, "Business Innovation Visa", null, "12-18 months", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarouselImages_ImageUploadId",
                table: "CarouselImages",
                column: "ImageUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ImageUploadId",
                table: "Offers",
                column: "ImageUploadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarouselImages");

            migrationBuilder.DropTable(
                name: "ContactInquiries");

            migrationBuilder.DropTable(
                name: "HomePageContents");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VisaServices");

            migrationBuilder.DropTable(
                name: "ImageUploads");
        }
    }
}
