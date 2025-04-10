using PennyPal.Dtos;
using PennyPal.Models;

namespace PennyPal.Services
{
    public interface IExpenseCategoryService
    {
        Task<IEnumerable<ExpenseCategoryDto>> GetUserExpenseCategories(int userId);
        Task<ExpenseCategory?> GetExpenseCategoryById(int expenseCategoryId, int userId); 
         Task<ExpenseCategory> AddExpenseCategory(ExpenseCategoryForRegistrationDto expenseCategory);
        Task UpdateExpenseCategory(ExpenseCategoryForUpdateDto expenseCategory, int userId);
        Task DeleteExpenseCategory(int expenseCategoryId, int userId);

    }
}