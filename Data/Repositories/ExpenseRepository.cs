using Microsoft.EntityFrameworkCore;
using PennyPal.Data;
using PennyPal.Dtos;
using PennyPal.Models;

namespace PennyPal.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly DataContextEF _entityFramework;

        public ExpenseRepository(DataContextEF entityFramework)
        {
            _entityFramework = entityFramework;
        }

        public async Task<IEnumerable<Expense>> GetExpensesByFilters(int userId,ExpenseFilterDto filters)
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
    }
}