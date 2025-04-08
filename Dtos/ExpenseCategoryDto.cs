namespace PennyPal.Dtos
{
    public class ExpenseCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal MonthlyBudget { get; set; }
    public required List<ExpenseDto> Expenses { get; set; }
}
}