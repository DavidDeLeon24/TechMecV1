using System.ComponentModel.DataAnnotations;

namespace TechMec.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Employee ID")] // Changed from Email to Employee ID
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}