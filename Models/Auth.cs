using System.ComponentModel.DataAnnotations;

namespace PennyPal.Models
{
    public partial class Auth
    {
        [Key]
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public required byte[] PasswordHash { get; set; }
        [Required]
        public required byte[] PasswordSalt { get; set; }
        public string Role { get; set; } = "user";

        public virtual User? User { get; set; }
    }
}