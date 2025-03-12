using System.ComponentModel.DataAnnotations;

namespace PennyPal.Models
{
    public partial class User
    {
        [Key]
        [Required]
        public int Id {get; set;}
        [Required]
        public required string Lastname {get; set;}
        [Required]
        public required string Firstname {get; set;} 
        [Required]
        public required string Email {get; set;} 

        public virtual Auth? Auth {get; set;}
        public virtual ICollection<ExpenseCategory>? ExpenseCategories {get; set;}

        public virtual ICollection<Expense>? Expenses {get; set;}
    }
}