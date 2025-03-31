using System.ComponentModel.DataAnnotations;

namespace PennyPal.Models
{
    public partial class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Lastname { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Firstname { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public required string Email { get; set; }

        public Auth Auth { get; set; } = null!;
        public ICollection<ExpenseCategory> ExpenseCategories { get; set; } = new List<ExpenseCategory>();

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}