using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace visa_consulatant.Migrations
{
    /// <inheritdoc />
    public partial class CreateAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Mobile = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Website = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    WorkingHours = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactInquiries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    VisaType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInquiries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomePageContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    HeroImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    LogoUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    BannerImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    WelcomeMessage = table.Column<string>(type: "TEXT", nullable: true),
                    ServicesOverview = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", nullable: false),
                    ImageData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    ImageType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UploadedBy = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageUploads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisaServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProcessingTime = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisaServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarouselImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ImageUploadId = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUploadId = table.Column<int>(type: "INTEGER", nullable: true),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false)
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
                table: "ContactInfos",
                columns: new[] { "Id", "Address", "City", "CompanyName", "Country", "CreatedAt", "Description", "Email", "IsActive", "Mobile", "Phone", "PostalCode", "State", "UpdatedAt", "Website", "WorkingHours" },
                values: new object[] { 1, "123 Immigration Street", "Melbourne", "Guru Kirpa Immigration Services", "Australia", new DateTime(2025, 8, 10, 11, 35, 57, 934, DateTimeKind.Utc).AddTicks(4949), "Your trusted partner for all Australian immigration services. We provide expert guidance and support for your visa applications to Australia.", "info@gurukirpa.com", true, "+61 400 123 456", "+61 3 1234 5678", "3000", "Victoria", null, "www.gurukirpa.com", "Monday - Friday: 9:00 AM - 6:00 PM" });

            migrationBuilder.InsertData(
                table: "HomePageContents",
                columns: new[] { "Id", "BannerImageUrl", "Description", "HeroImageUrl", "LastUpdated", "LogoUrl", "ServicesOverview", "Title", "UpdatedBy", "WelcomeMessage" },
                values: new object[] { 1, null, "Your trusted partner for all Australian immigration services. We provide expert guidance and support for your visa applications to Australia.", null, new DateTime(2025, 8, 10, 11, 35, 57, 933, DateTimeKind.Utc).AddTicks(3289), null, "We offer comprehensive Australian immigration services including skilled migration, student visas, partner visas, tourist visas, and permanent residency applications.", "Guru Kirpa Immigration - Your Gateway to Australia", "System", "Welcome to Guru Kirpa Immigration - Your Gateway to Australia" });

            migrationBuilder.InsertData(
                table: "Offers",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "DiscountPercentage", "ImageUploadId", "IsActive", "Title", "ValidFrom", "ValidUntil" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 10, 11, 35, 57, 934, DateTimeKind.Utc).AddTicks(107), "System", "Get 20% off on student visa processing fees. Limited time offer for new students!", 20.00m, null, true, "Student Visa Special Offer", new DateTime(2025, 8, 10, 11, 35, 57, 933, DateTimeKind.Utc).AddTicks(9036), new DateTime(2025, 11, 10, 11, 35, 57, 933, DateTimeKind.Utc).AddTicks(9418) },
                    { 2, new DateTime(2025, 8, 10, 11, 35, 57, 934, DateTimeKind.Utc).AddTicks(589), "System", "Complete family immigration package with special rates for families of 3 or more members.", 15.00m, null, true, "Family Package Deal", new DateTime(2025, 8, 10, 11, 35, 57, 934, DateTimeKind.Utc).AddTicks(580), new DateTime(2026, 2, 10, 11, 35, 57, 934, DateTimeKind.Utc).AddTicks(581) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, new DateTime(2025, 8, 10, 11, 35, 57, 932, DateTimeKind.Utc).AddTicks(759), "admin@gurukirpa.com", true, "$2a$11$bsZg2pzuz5NKNtCHSV7afO3gB1nYBaihzuAUVvf8HUvs7PFNMmPom", "Admin", "admin" });

            migrationBuilder.InsertData(
                table: "VisaServices",
                columns: new[] { "Id", "Country", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Price", "ProcessingTime", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Australia", new DateTime(2025, 8, 10, 11, 35, 57, 933, DateTimeKind.Utc).AddTicks(6228), "Professional skilled migration services for qualified workers seeking permanent residency in Australia through points-based system.", null, true, "Skilled Migration Visa", null, "6-12 months", null },
                    { 2, "Australia", new DateTime(2025, 8, 10, 11, 35, 57, 933, DateTimeKind.Utc).AddTicks(6460), "Comprehensive student visa services for international students wishing to study at Australian educational institutions.", null, true, "Student Visa (Subclass 500)", null, "1-3 months", null },
                    { 3, "Australia", new DateTime(2025, 8, 10, 11, 35, 57, 933, DateTimeKind.Utc).AddTicks(6463), "Expert guidance for partner visa applications including spouse, de facto, and prospective marriage visas.", null, true, "Partner Visa", null, "12-24 months", null },
                    { 4, "Australia", new DateTime(2025, 8, 10, 11, 35, 57, 933, DateTimeKind.Utc).AddTicks(6466), "Tourist and visitor visa services for short-term stays in Australia for tourism, business, or family visits.", null, true, "Tourist Visa (Subclass 600)", null, "2-4 weeks", null },
                    { 5, "Australia", new DateTime(2025, 8, 10, 11, 35, 57, 933, DateTimeKind.Utc).AddTicks(6469), "Temporary skill shortage visa services for skilled workers sponsored by Australian employers.", null, true, "Work Visa (Subclass 482)", null, "3-6 months", null },
                    { 6, "Australia", new DateTime(2025, 8, 10, 11, 35, 57, 933, DateTimeKind.Utc).AddTicks(6472), "Business and investment visa services for entrepreneurs and investors seeking to establish businesses in Australia.", null, true, "Business Innovation Visa", null, "12-18 months", null }
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
                name: "ContactInfos");

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
