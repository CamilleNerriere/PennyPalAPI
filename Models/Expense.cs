using System.ComponentModel.DataAnnotations;

namespace PennyPal.Models
{
    public partial class Expense
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public required int UserId { get; set; }
        [Required]
        public required int CategoryId { get; set; }
        public string? Name { get; set; }
        [Required]
        public required decimal Amount { get; set; }
        [Required]
        public required DateTime Date {get; set;}

        public virtual User? User { get; set; }
        public virtual ExpenseCategory? ExpenseCategory { get; set; }

    }
}