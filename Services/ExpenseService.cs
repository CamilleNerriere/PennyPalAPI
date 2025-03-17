using PennyPal.Dtos;
using PennyPal.Models;
using PennyPal.Repositories;

namespace PennyPal.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<IEnumerable<Expense>> GetExpensesByFilters(int userId, ExpenseFilterDto filters)
        {
            return await _expenseRepository.GetExpensesByFilters(userId, filters); 
        }
    }
}