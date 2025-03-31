using System.ComponentModel.DataAnnotations;

namespace PennyPal.Models
{
    public partial class ExpenseCategory
    {
        [Key]
        public int Id {get; set;}
        public required int UserId {get; set;}
        [Required]
        public required string Name {get; set;}
        public required decimal MonthlyBudget {get; set;}

        public  User User {get; set;} = null!;
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    
    }
}