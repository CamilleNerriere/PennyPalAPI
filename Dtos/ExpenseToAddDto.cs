using System.ComponentModel.DataAnnotations;

namespace PennyPal.Dtos
{
    public partial class ExpenseToAddDto
    {
        public int UserId { get; set; }
        [Required]
        public required int CategoryId { get; set; }
        public string? Name { get; set; }
        [Required]
        public required decimal Amount { get; set; }
        [Required]
        public required DateTime Date {get; set;}
    }
}