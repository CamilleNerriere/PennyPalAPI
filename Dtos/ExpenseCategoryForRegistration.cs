using System.ComponentModel.DataAnnotations;

namespace PennyPal.Dtos
{
    public partial class ExpenseCategoryForRegistrationDto
    {
        [Required(ErrorMessage = "You must provide a user")]
        public required int UserId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ-]+$", ErrorMessage = "Lastname contains invalid characters.")]
        public required string Name { get; set; }
        [Required(ErrorMessage ="Monthly budget is required.")]
        [RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage ="Monthly budget can only contain digits")]
        public required decimal MonthlyBudget { get; set; }

    }
}