using PennyPal.Data.Repositories;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Models;

namespace PennyPal.Services
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;

        public ExpenseCategoryService(IExpenseCategoryRepository expenseCategoryRepository)
        {
            _expenseCategoryRepository = expenseCategoryRepository;
        }

        public async Task<List<ExpenseCategory>> GetUserExpenseCategories(int userId)
        {
            return await _expenseCategoryRepository.GetUserExpenseCategories(userId);
        }

        public async Task<ExpenseCategory?> GetExpenseCategoryById(int expenseCategoryId, int userId)
        {   

            ExpenseCategory? category =  await _expenseCategoryRepository.GetExpenseCategoryById(expenseCategoryId);

            if(category != null && category.UserId != userId)
            {
                throw new Unauthorized(401, "Unauthorized");
            }

            return category;
        }

        public async Task AddExpenseCategory(ExpenseCategoryForRegistrationDto expenseCategory)
        {
            await _expenseCategoryRepository.AddExpenseCategory(expenseCategory);
        }

        public async Task UpdateExpenseCategory(ExpenseCategoryForUpdateDto expenseCategory, int userId)
        {
            ExpenseCategory expenseCategoryToUpdate = await _expenseCategoryRepository.GetExpenseCategoryById(expenseCategory.Id)
                ?? throw new NotFoundException("Category Not Found");
            
            if(expenseCategoryToUpdate.UserId != userId)
            {
                throw new Unauthorized(401, "Unauthorized operation");
            }

            await _expenseCategoryRepository.UpdateExpenseCategory(expenseCategory);

        }

        public async Task DeleteExpenseCategory(int expenseCategoryId, int userId)
        {
            ExpenseCategory? expenseCategoryToDelete = await _expenseCategoryRepository.GetExpenseCategoryById(expenseCategoryId)
                ?? throw new NotFoundException("Expense Cateogy not found");
            
            if(expenseCategoryToDelete.UserId != userId)
            {
                throw new CustomValidationException("Not allowed to delete this category");
            }

            await _expenseCategoryRepository.DeleteExpenseCategory(expenseCategoryId);
        }

    }
}