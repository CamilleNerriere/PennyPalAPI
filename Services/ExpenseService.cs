using System.Reflection.Metadata.Ecma335;
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

        public async Task<decimal> GetExpensesTendances(int userId,ExpenseTendancesFilterDto filters)
        {
            IEnumerable<Expense> expenses = await _expenseRepository.GetExpensesTendances(userId, filters);

            decimal sum = 0;

            int numberOfExpenses = expenses.Count();

            foreach (Expense expense in expenses)
            {
                sum += expense.Amount;
            }
            return Math.Round(sum/numberOfExpenses, 2);
        }
    }
}