
using System.ComponentModel.DataAnnotations;

namespace PennyPal.Models
{
    public partial class RefreshToken 
    {
        [Key]
        public int Id {get; set;}

        [Required]
        public string Token {get; set;} = String.Empty;

        [Required]
        public DateTime Expires {get; set;}

        [Required]
        public DateTime CreatedAt {get; set;}

        [Required]
        public string CreatedByIp {get; set;} = String.Empty;

        public bool Revoked {get; set;}

        public string? ReplacedByToken {get; set;}

        public DateTime SessionExpiresAt { get; set; }


        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}