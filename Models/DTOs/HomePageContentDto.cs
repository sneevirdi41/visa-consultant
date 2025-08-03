using System.ComponentModel.DataAnnotations;

namespace visa_consulatant.Models.DTOs
{
    public class HomePageContentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? HeroImageUrl { get; set; }
        public string? LogoUrl { get; set; }
        public string? BannerImageUrl { get; set; }
        public string? WelcomeMessage { get; set; }
        public string? ServicesOverview { get; set; }
        public DateTime LastUpdated { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }

    public class UpdateHomePageContentDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? HeroImageUrl { get; set; }
        
        [StringLength(500)]
        public string? LogoUrl { get; set; }
        
        [StringLength(500)]
        public string? BannerImageUrl { get; set; }
        
        public string? WelcomeMessage { get; set; }
        
        public string? ServicesOverview { get; set; }
    }
} 