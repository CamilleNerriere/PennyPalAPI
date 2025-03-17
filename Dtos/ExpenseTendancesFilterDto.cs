namespace PennyPal.Dtos
{
    public class ExpenseTendancesFilterDto
    {
        public int? Month1 { get; set; } 
        public int? Year1 { get; set; }   
        public int? Month2 { get; set; } 
        public int? Year2 { get; set; }  
        public int? CategoryId { get; set; } 
    }
}