using PennyPal.Dtos;
using PennyPal.Models;

namespace PennyPal.Data.Repositories
{
    public interface IExpenseRepository
    {
        Task<Expense> GetExpenseById(int expenseId);
        Task<List<Expense>>  GetUserExpense(int userId);
        Task<IEnumerable<Expense>> GetExpensesByFilters(int userId, ExpenseFilterDto filters);
        Task<IEnumerable<Expense>> GetExpensesTendances(int userId, ExpenseTendancesFilterDto filters);
        Task AddExpense(Expense expense);
        Task UpdateExpense(Expense expense);
        Task DeleteExpense(int expenseId);
    }
}