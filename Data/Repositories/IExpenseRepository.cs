using PennyPal.Dtos;
using PennyPal.Models;

namespace PennyPal.Repositories
{
    public interface IExpenseRepository 
    {
        Task<IEnumerable<Expense>> GetExpensesByFilters(int userId, ExpenseFilterDto filters);
    }
}