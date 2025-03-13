using System.ComponentModel.DataAnnotations;

namespace PennyPal.Dtos
{
    public partial class UserUpdateDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [MaxLength(50, ErrorMessage = "Lastname cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ-]+$", ErrorMessage = "Lastname contains invalid characters.")]
        public required string Lastname { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [MaxLength(50, ErrorMessage = "Lastname cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ-]+$", ErrorMessage = "Lastname contains invalid characters.")]
        public required string Firstname { get; set; }

    }
}