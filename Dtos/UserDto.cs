using System.ComponentModel.DataAnnotations;

namespace PennyPal.Dtos
{
    public partial class UserDto
    {
        [Required(ErrorMessage = "Lastname is required")]
        [MaxLength(50, ErrorMessage = "Lastname cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ-]+$", ErrorMessage = "Lastname contains invalid characters.")]
        public required string Lastname { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [MaxLength(50, ErrorMessage = "Lastname cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ-]+$", ErrorMessage = "Lastname contains invalid characters.")]
        public required string Firstname { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

    }
}