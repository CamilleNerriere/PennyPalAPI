using PennyPal.Dtos;
using PennyPal.Models;

namespace PennyPal.Services
{
    public interface IExpenseCategoryService
    {
        // Tous les domaines d'un utilisateur
        Task<IEnumerable<ExpenseCategoryDto>> GetUserExpenseCategories(int userId);
        // Un domaine par id
        Task<ExpenseCategory?> GetExpenseCategoryById(int expenseCategoryId, int userId); 
        // Créer un domaine 
        Task AddExpenseCategory(ExpenseCategoryForRegistrationDto expenseCategory);
        // Modifier un domaine 
        Task UpdateExpenseCategory(ExpenseCategoryForUpdateDto expenseCategory, int userId);
        // Supprimer un domaine 
        Task DeleteExpenseCategory(int expenseCategoryId, int userId);

    }
}