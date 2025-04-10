using PennyPal.Data.Repositories;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Models;

namespace PennyPal.Services
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;

        private readonly ILogger<ExpenseCategoryService> _logger;

        public ExpenseCategoryService(IExpenseCategoryRepository expenseCategoryRepository, ILogger<ExpenseCategoryService> logger)
        {
            _expenseCategoryRepository = expenseCategoryRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ExpenseCategoryDto>> GetUserExpenseCategories(int userId)
        {
            var categories = await _expenseCategoryRepository.GetUserExpenseCategories(userId);

            var result = categories.Select(c => new ExpenseCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                MonthlyBudget = c.MonthlyBudget,
                Expenses = c.Expenses.Select(exp => new ExpenseDto
                {
                    Id = exp.Id,
                    Name = exp.Name ?? "",
                    Amount = exp.Amount,
                    Date = exp.Date
                }).ToList()
            });

            return result;
        }

        public async Task<ExpenseCategory?> GetExpenseCategoryById(int expenseCategoryId, int userId)
        {

            var category = await _expenseCategoryRepository.GetExpenseCategoryById(expenseCategoryId);

            if (category != null && category.UserId != userId)
            {
                _logger.LogError("Unauthorized attempt to access Expense Category {expenseCategoryId} by user {userID}", expenseCategoryId, userId);
                throw new Unauthorized(401, "Unauthorized");
            }

            return category;
        }

        public async Task<ExpenseCategory> AddExpenseCategory(ExpenseCategoryForRegistrationDto expenseCategory)
        {
            return await _expenseCategoryRepository.AddExpenseCategory(expenseCategory);
        }

        public async Task UpdateExpenseCategory(ExpenseCategoryForUpdateDto expenseCategory, int userId)
        {
            var expenseCategoryToUpdate = await _expenseCategoryRepository.GetExpenseCategoryById(expenseCategory.Id)
                ?? throw new NotFoundException("Category Not Found");

            if (expenseCategoryToUpdate.UserId != userId)
            {
                _logger.LogError("Unauthorized attempt to update Expense Category {expenseCategoryId} by user {userID}", expenseCategory.Id, userId);

                throw new Unauthorized(401, "Unauthorized operation");
            }

            await _expenseCategoryRepository.UpdateExpenseCategory(expenseCategory);

        }

        public async Task DeleteExpenseCategory(int expenseCategoryId, int userId)
        {
            var expenseCategoryToDelete = await _expenseCategoryRepository.GetExpenseCategoryById(expenseCategoryId)
                ?? throw new NotFoundException("Expense Cateogy not found");

            if (expenseCategoryToDelete.UserId != userId)
            {
                _logger.LogError("Unauthorized attempt to delete Expense Category {expenseCategoryId} by user {userID}", expenseCategoryId, userId);

                throw new CustomValidationException("Not allowed to delete this category");
            }

            await _expenseCategoryRepository.DeleteExpenseCategory(expenseCategoryId);
        }

    }
}