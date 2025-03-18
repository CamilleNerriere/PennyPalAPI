using PennyPal.Dtos;
using PennyPal.Models;

namespace PennyPal.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<Expense>> GetExpensesByFilters(int userId, ExpenseFilterDto filters);
        Task<decimal> GetExpensesTendances(int userId,ExpenseTendancesFilterDto filters);
        Task AddExpense(ExpenseToAddDto expense);
        Task UpdateExpense(ExpenseToUpdateDto expense, int userId);
        Task DeleteExpense(int expenseId, int userId);
    }
}