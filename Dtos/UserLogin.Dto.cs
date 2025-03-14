using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace PennyPal.Dtos
{
    public partial class UserLoginDto
    {
        [Key]
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}