using System.ComponentModel.DataAnnotations;

namespace visa_consulatant.Models
{
    public class HomePageContent
    {
        public int Id { get; set; }
        
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
        
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        
        public string UpdatedBy { get; set; } = string.Empty;
    }
} 