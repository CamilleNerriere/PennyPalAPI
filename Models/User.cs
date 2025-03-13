using System.ComponentModel.DataAnnotations;

namespace PennyPal.Models
{
    public partial class User
    {
        [Key]
        [Required]
        public int Id {get; set;}
        [Required]
        [MaxLength(50, ErrorMessage ="Lastname cannot exceed 50 characters.")]
        public required string Lastname {get; set;}
        [Required]
        [MaxLength(50, ErrorMessage ="Firstname cannot exceed 50 characters.")]
        public required string Firstname {get; set;} 
        [Required]
        [MaxLength(100, ErrorMessage ="Email cannot exceed 50 characters.")]
        [EmailAddress (ErrorMessage ="Invalid Email Address")]
        public required string Email {get; set;} 

        public virtual Auth? Auth {get; set;}
        public virtual ICollection<ExpenseCategory>? ExpenseCategories {get; set;}

        public virtual ICollection<Expense>? Expenses {get; set;}
    }
}