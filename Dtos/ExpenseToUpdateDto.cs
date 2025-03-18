using System.ComponentModel.DataAnnotations;

namespace PennyPal.Dtos
{
    public partial class ExpenseToUpdateDto
    {
        [Required]
        public required int Id { get; set; }
        [Required]
        public required int UserId { get; set; }
        [Required]
        public required int CategoryId { get; set; }
        public string? Name { get; set; }
        [Required]
        public required decimal Amount { get; set; }
        [Required]
        public required DateTime Date { get; set; }
    }
}