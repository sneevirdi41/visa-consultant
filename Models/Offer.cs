using System.ComponentModel.DataAnnotations;

namespace visa_consulatant.Models
{
    public class Offer
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public int? ImageUploadId { get; set; }
        
        public ImageUpload? ImageUpload { get; set; }
        
        public decimal? DiscountPercentage { get; set; }
        
        public DateTime? ValidFrom { get; set; }
        
        public DateTime? ValidUntil { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public string CreatedBy { get; set; } = string.Empty;
    }
} 