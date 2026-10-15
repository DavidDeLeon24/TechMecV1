using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMec.Models
{
    public class Handover
    {
        [Key]
        public int HandoverId { get; set; }

        [Required]
        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        [Required]
        [ForeignKey(nameof(FromUser))]
        public string FromUserId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(ToUser))]
        public string ToUserId { get; set; } = string.Empty;

        [Required]
        public HandoverShift ShiftType { get; set; }

        [StringLength(2000)]
        [Display(Name = "Special Instructions")]
        public string? SpecialInstructions { get; set; }

        public DateTime HandoverTime { get; set; } = DateTime.UtcNow;

        // Acknowledgment
        public bool IsAcknowledged { get; set; } = false;
        public DateTime? AcknowledgedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Patient Patient { get; set; } = null!;
        public ApplicationUser FromUser { get; set; } = null!;
        public ApplicationUser ToUser { get; set; } = null!;
    }
}