using System.ComponentModel.DataAnnotations;

namespace visa_consulatant.Models
{
    public class ContactInquiry
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        public string Message { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Subject { get; set; }
        
        [StringLength(100)]
        public string? VisaType { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsRead { get; set; } = false;
        
        public DateTime? ReadAt { get; set; }
    }
} 