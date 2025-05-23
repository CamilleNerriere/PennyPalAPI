using System.ComponentModel.DataAnnotations;

namespace PennyPal.Dtos
{
    public partial class ExpenseCategoryForUpdateDto
    {

        public int UserId { get; set; }
        [Required(ErrorMessage = "You must provide a category")]
        public required int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Monthly budget is required.")]
        [RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage = "Monthly budget can only contain digits")]
        public required decimal MonthlyBudget { get; set; }

    }
}