using System.ComponentModel.DataAnnotations;

namespace visa_consulatant.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string State { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string PostalCode { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string Mobile { get; set; } = string.Empty;
        
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string Website { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string WorkingHours { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
}

