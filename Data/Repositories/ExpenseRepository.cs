using Microsoft.EntityFrameworkCore;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Models;

namespace PennyPal.Data.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly DataContextEF _entityFramework;

        public ExpenseRepository(DataContextEF entityFramework)
        {
            _entityFramework = entityFramework;
        }

        public async Task<Expense> GetExpenseById(int expenseId)
        {
            Expense expense = await _entityFramework.Expense.FindAsync(expenseId) 
                ?? throw new NotFoundException("Expense not found");

            return expense;
        }
        public async Task<IEnumerable<Expense>> GetExpensesByFilters(int userId, ExpenseFilterDto filters)
        {
            IQueryable<Expense> query = _entityFramework.Expense.AsQueryable();

            if (filters.Month.HasValue)
            {
                query = query.Where(e => e.Date.Month == filters.Month.Value);
            }

            if (filters.Year.HasValue)
            {
                query = query.Where(e => e.Date.Year == filters.Year.Value);
            }

            if (filters.CategoryId.HasValue)
            {
                query = query.Where(e => e.CategoryId == filters.CategoryId.Value);
            }

            query = query.Where(e => e.UserId == userId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesTendances(int userId, ExpenseTendancesFilterDto filters)
        {
            IQueryable<Expense> query = _entityFramework.Expense.AsQueryable();

            if (filters.Month1.HasValue)
            {
                query = query.Where(e => e.Date.Month == filters.Month1.Value);
            }

            if (filters.Year1.HasValue)
            {
                query = query.Where(e => e.Date.Year == filters.Year1.Value);
            }

            if (filters.Month2.HasValue)
            {
                query = query.Where(e => e.Date.Month == filters.Month2.Value);
            }

            if (filters.Year2.HasValue)
            {
                query = query.Where(e => e.Date.Year == filters.Year2.Value);
            }

            if (filters.CategoryId.HasValue)
            {
                query = query.Where(e => e.CategoryId == filters.CategoryId.Value);
            }

            query = query.Where(e => e.UserId == userId);

            return await query.ToListAsync();
        }

        public async Task AddExpense(Expense expense)
        {
            await _entityFramework.Expense.AddAsync(expense);
            await _entityFramework.SaveChangesAsync();
        }

        public async Task UpdateExpense(Expense expense)
        {
            Expense expenseToUpdate = await _entityFramework.Expense.FindAsync(expense.Id)
                ?? throw new NotFoundException("Expense not found");

            expenseToUpdate.Amount = expense.Amount != 0 ? expense.Amount : expenseToUpdate.Amount;
            expenseToUpdate.CategoryId = expense.CategoryId != expenseToUpdate.CategoryId ?
                expense.CategoryId : expenseToUpdate.CategoryId;
            expenseToUpdate.Date = expense.Date != expenseToUpdate.Date ? expense.Date : expenseToUpdate.Date;
            expenseToUpdate.Name = expense.Name ?? expenseToUpdate.Name;
            await _entityFramework.SaveChangesAsync();
        }

        public async Task DeleteExpense(int expenseId)
        {
            Expense? expense = await _entityFramework.Expense.FindAsync(expenseId) ?? throw new NotFoundException("Expense not found");
            _entityFramework.Remove(expense);
            await _entityFramework.SaveChangesAsync();
        }
    }
}