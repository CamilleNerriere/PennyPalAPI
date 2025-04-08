using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace PennyPal.Dtos
{
    public partial class UserUpdatePasswordDto
    {
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public required string ConfirmPassword { get; set; }
    }
}