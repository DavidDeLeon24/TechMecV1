using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TechMec.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Department { get; set; }

        [StringLength(50)]
        [Display(Name = "License Number")]
        public string? LicenseNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public ICollection<Handover> SentHandovers { get; set; } = new List<Handover>();
        public ICollection<Handover> ReceivedHandovers { get; set; } = new List<Handover>();
    }
}