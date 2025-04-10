using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Models;

namespace PennyPal.Data.Repositories
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly DataContextEF _entityFramework;
        private readonly IMapper _mapper;

        public ExpenseCategoryRepository(DataContextEF entityFramework)
        {
            _entityFramework = entityFramework;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ExpenseCategoryForRegistrationDto, ExpenseCategory>();
            }));
        }

        public async Task<List<ExpenseCategory>> GetUserExpenseCategories(int userId)
        {
            return await _entityFramework.ExpenseCategory.Where(e => e.UserId == userId).Include(e => e.Expenses).ToListAsync();
        }

        public async Task<ExpenseCategory?> GetExpenseCategoryById(int expenseCategoryId)
        {
            return await _entityFramework.ExpenseCategory.FindAsync(expenseCategoryId);
        }

        public async Task<ExpenseCategory> AddExpenseCategory(ExpenseCategoryForRegistrationDto expenseCategory)
        {
            if(expenseCategory == null)
            {
                throw new CustomValidationException("Expense Category is null");
            }

            var expenseCategoryMap = _mapper.Map<ExpenseCategory>(expenseCategory);

            await _entityFramework.ExpenseCategory.AddAsync(expenseCategoryMap);
            await _entityFramework.SaveChangesAsync();

            return expenseCategoryMap;
        }

        public async Task UpdateExpenseCategory(ExpenseCategoryForUpdateDto expenseCategory)
        {
            if(expenseCategory == null)
            {
                throw new CustomValidationException("Expense Category null or empty");
            }

            var expenseCategoryToUpdate = await _entityFramework.ExpenseCategory.FindAsync(expenseCategory.Id) ?? throw new NotFoundException("Expense Category Not Found");
            expenseCategoryToUpdate.Name = expenseCategory.Name ?? expenseCategoryToUpdate.Name;
            if(expenseCategory.MonthlyBudget != expenseCategoryToUpdate.MonthlyBudget)
            {
                expenseCategoryToUpdate.MonthlyBudget = expenseCategory.MonthlyBudget;
            }
            await _entityFramework.SaveChangesAsync();
        }

        public async Task DeleteExpenseCategory(int expenseCategoryId)
        {
            var expenseCategory = await _entityFramework.ExpenseCategory.FindAsync(expenseCategoryId) ?? throw new NotFoundException("Expense Category not found");
            _entityFramework.Remove(expenseCategory);
            await _entityFramework.SaveChangesAsync();
        }
    }
}