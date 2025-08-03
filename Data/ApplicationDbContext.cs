using Microsoft.EntityFrameworkCore;
using visa_consulatant.Models;

namespace visa_consulatant.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<HomePageContent> HomePageContents { get; set; } = null!;
        public DbSet<VisaService> VisaServices { get; set; } = null!;
        public DbSet<ContactInquiry> ContactInquiries { get; set; } = null!;
        public DbSet<ImageUpload> ImageUploads { get; set; } = null!;
        public DbSet<CarouselImage> CarouselImages { get; set; } = null!;
        public DbSet<Offer> Offers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for PostgreSQL
            modelBuilder.Entity<VisaService>()
                .Property(v => v.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Offer>()
                .Property(o => o.DiscountPercentage)
                .HasColumnType("decimal(5,2)");

            // Configure relationships
            modelBuilder.Entity<CarouselImage>()
                .HasOne(c => c.ImageUpload)
                .WithMany()
                .HasForeignKey(c => c.ImageUploadId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Offer>()
                .HasOne(o => o.ImageUpload)
                .WithMany()
                .HasForeignKey(o => o.ImageUploadId)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed default admin user
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@gurukirpa.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            });

            // Seed default homepage content
            modelBuilder.Entity<HomePageContent>().HasData(new HomePageContent
            {
                Id = 1,
                Title = "Guru Kirpa Immigration - Your Gateway to Australia",
                Description = "Your trusted partner for all Australian immigration services. We provide expert guidance and support for your visa applications to Australia.",
                WelcomeMessage = "Welcome to Guru Kirpa Immigration - Your Gateway to Australia",
                ServicesOverview = "We offer comprehensive Australian immigration services including skilled migration, student visas, partner visas, tourist visas, and permanent residency applications.",
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = "System"
            });

            // Seed Australian immigration services
            modelBuilder.Entity<VisaService>().HasData(
                new VisaService
                {
                    Id = 1,
                    Name = "Skilled Migration Visa",
                    Description = "Professional skilled migration services for qualified workers seeking permanent residency in Australia through points-based system.",
                    ProcessingTime = "6-12 months",
                    Country = "Australia",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new VisaService
                {
                    Id = 2,
                    Name = "Student Visa (Subclass 500)",
                    Description = "Comprehensive student visa services for international students wishing to study at Australian educational institutions.",
                    ProcessingTime = "1-3 months",
                    Country = "Australia",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new VisaService
                {
                    Id = 3,
                    Name = "Partner Visa",
                    Description = "Expert guidance for partner visa applications including spouse, de facto, and prospective marriage visas.",
                    ProcessingTime = "12-24 months",
                    Country = "Australia",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new VisaService
                {
                    Id = 4,
                    Name = "Tourist Visa (Subclass 600)",
                    Description = "Tourist and visitor visa services for short-term stays in Australia for tourism, business, or family visits.",
                    ProcessingTime = "2-4 weeks",
                    Country = "Australia",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new VisaService
                {
                    Id = 5,
                    Name = "Work Visa (Subclass 482)",
                    Description = "Temporary skill shortage visa services for skilled workers sponsored by Australian employers.",
                    ProcessingTime = "3-6 months",
                    Country = "Australia",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new VisaService
                {
                    Id = 6,
                    Name = "Business Innovation Visa",
                    Description = "Business and investment visa services for entrepreneurs and investors seeking to establish businesses in Australia.",
                    ProcessingTime = "12-18 months",
                    Country = "Australia",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed sample offers
            modelBuilder.Entity<Offer>().HasData(
                new Offer
                {
                    Id = 1,
                    Title = "Student Visa Special Offer",
                    Description = "Get 20% off on student visa processing fees. Limited time offer for new students!",
                    DiscountPercentage = 20.00m,
                    ValidFrom = DateTime.UtcNow,
                    ValidUntil = DateTime.UtcNow.AddMonths(3),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Offer
                {
                    Id = 2,
                    Title = "Family Package Deal",
                    Description = "Complete family immigration package with special rates for families of 3 or more members.",
                    DiscountPercentage = 15.00m,
                    ValidFrom = DateTime.UtcNow,
                    ValidUntil = DateTime.UtcNow.AddMonths(6),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            );
        }
    }
} 