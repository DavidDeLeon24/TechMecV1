using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMec.Models
{
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        [ForeignKey(nameof(Handover))]
        public int HandoverId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Priority { get; set; } = "Medium"; // High, Medium, Low

        public bool IsCompleted { get; set; } = false;

        public DateTime? CompletedAt { get; set; }

        public string? CompletedByUserId { get; set; }

        // Navigation Property
        public Handover Handover { get; set; } = null!;
    }
}