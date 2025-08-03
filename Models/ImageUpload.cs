using System.ComponentModel.DataAnnotations;

namespace visa_consulatant.Models
{
    public class ImageUpload
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        public string ContentType { get; set; } = string.Empty;
        
        [Required]
        public byte[] ImageData { get; set; } = new byte[0];
        
        [StringLength(100)]
        public string? ImageType { get; set; } // "logo", "carousel", "hero", "banner"
        
        [StringLength(200)]
        public string? Title { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        public string UploadedBy { get; set; } = string.Empty;
    }
} 