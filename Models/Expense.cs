using System.ComponentModel.DataAnnotations;

namespace PennyPal.Models
{
    public partial class Expense
    {
        [Key]
        public int Id { get; set; }
        public required int UserId { get; set; }
        public required int CategoryId { get; set; }
        public string? Name { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime Date {get; set;}

        public User User { get; set; } = null!;
        public ExpenseCategory ExpenseCategory { get; set; } = null!;

    }
}