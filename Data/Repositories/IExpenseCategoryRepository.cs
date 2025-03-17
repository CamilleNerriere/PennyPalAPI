using PennyPal.Dtos;
using PennyPal.Models;

namespace PennyPal.Data.Repositories
{
    public interface IExpenseCategoryRepository
    {
        // Tous les domaines d'un utilisateur
        Task<List<ExpenseCategory>> GetUserExpenseCategories(int userId);
        // Un domaine par id
        Task<ExpenseCategory?> GetExpenseCategoryById(int expenseCategoryId); 
        // Créer un domaine 
        Task AddExpenseCategory(ExpenseCategoryForRegistrationDto expenseCategory);
        // Modifier un domaine 
        Task UpdateExpenseCategory(ExpenseCategoryForUpdateDto expenseCategory);
        // Supprimer un domaine 
        Task DeleteExpenseCategory(int expenseCategoryId);

    }
}