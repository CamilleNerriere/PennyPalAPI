using System.ComponentModel.DataAnnotations;

namespace PennyPal.Models
{
    public partial class ExpenseCategory
    {
        [Key]
        [Required]
        public int Id {get; set;}
        [Required]
        public required int UserId {get; set;}
        [Required]
        public required string Name {get; set;}
        [Required]
        public required decimal MonthlyBudget {get; set;}

        public virtual User? User {get; set;}
        public virtual ICollection<Expense>? Expenses {get; set;}
    
    }
}