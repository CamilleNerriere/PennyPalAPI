using System.ComponentModel.DataAnnotations;

namespace PennyPal.Models
{
    public partial class User
    {
        [Key]
        public int Id {get; set;}
        [Required]
        [MaxLength(50)]
        public required string Lastname {get; set;}
        [Required]
        [MaxLength(50)]
        public required string Firstname {get; set;} 
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public required string Email {get; set;} 

        public virtual Auth? Auth {get; set;}
        public virtual ICollection<ExpenseCategory>? ExpenseCategories {get; set;}

        public virtual ICollection<Expense>? Expenses {get; set;}
    }
}