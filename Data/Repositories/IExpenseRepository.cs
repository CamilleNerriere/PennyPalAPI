using PennyPal.Dtos;
using PennyPal.Models;

namespace PennyPal.Repositories
{
    public interface IExpenseRepository 
    {
        Task<Expense> GetExpenseById(int expenseId);
        Task<IEnumerable<Expense>> GetExpensesByFilters(int userId, ExpenseFilterDto filters);
        Task<IEnumerable<Expense>> GetExpensesTendances(int userId,ExpenseTendancesFilterDto filters);
        Task AddExpense(Expense expense);
        Task UpdateExpense(Expense expense);
        Task DeleteExpense(int expenseId);
    }
}