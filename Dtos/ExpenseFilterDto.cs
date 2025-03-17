namespace PennyPal.Dtos
{
    public class ExpenseFilterDto
    {
        public int? Month { get; set; } 
        public int? Year { get; set; }   
        public int? CategoryId { get; set; } 
    }
}