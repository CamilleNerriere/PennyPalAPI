namespace PennyPal.Dtos
{
    public class ExpenseCategoryRemainDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal MonthlyBudget { get; set; }
        public decimal Remain { get; set; }
    }
}