using System.ComponentModel.DataAnnotations;

namespace visa_consulatant.Models
{
    public class VisaService
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? ImageUrl { get; set; }
        
        public decimal? Price { get; set; }
        
        [StringLength(100)]
        public string? ProcessingTime { get; set; }
        
        [StringLength(100)]
        public string? Country { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
} 