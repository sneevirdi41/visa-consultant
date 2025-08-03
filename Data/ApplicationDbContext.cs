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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for SQLite
            modelBuilder.Entity<VisaService>()
                .Property(v => v.Price)
                .HasPrecision(18, 2);

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
                Title = "Guru Kirpa Visa Consultancy",
                Description = "Your trusted partner for all visa and immigration services. We provide expert guidance and support for your visa applications.",
                WelcomeMessage = "Welcome to Guru Kirpa Visa Consultancy - Your Gateway to Global Opportunities",
                ServicesOverview = "We offer comprehensive visa services including tourist visas, student visas, work permits, and permanent residency applications.",
                LastUpdated = DateTime.UtcNow,
                UpdatedBy = "System"
            });

            // Seed some default visa services
            modelBuilder.Entity<VisaService>().HasData(
                new VisaService
                {
                    Id = 1,
                    Name = "Tourist Visa",
                    Description = "Comprehensive tourist visa services for various countries including USA, UK, Canada, Australia, and Schengen countries.",
                    ProcessingTime = "5-15 days",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new VisaService
                {
                    Id = 2,
                    Name = "Student Visa",
                    Description = "Expert guidance for student visa applications to study abroad in top universities worldwide.",
                    ProcessingTime = "10-30 days",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new VisaService
                {
                    Id = 3,
                    Name = "Work Permit",
                    Description = "Professional work permit and employment visa services for skilled workers and professionals.",
                    ProcessingTime = "15-45 days",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new VisaService
                {
                    Id = 4,
                    Name = "Permanent Residency",
                    Description = "Long-term immigration solutions including permanent residency and citizenship applications.",
                    ProcessingTime = "6-18 months",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
} 