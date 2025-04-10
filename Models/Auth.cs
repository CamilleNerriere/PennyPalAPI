using System.ComponentModel.DataAnnotations;

namespace PennyPal.Models
{
    public partial class Auth
    {
        [Key]
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        public User User { get; set; } = null!;
    }
}