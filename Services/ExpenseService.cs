using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Models;
using PennyPal.Repositories;

namespace PennyPal.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ExpenseToAddDto, Expense>();
                cfg.CreateMap<ExpenseToUpdateDto, Expense>();
            }));
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
        
        public async Task AddExpense(ExpenseToAddDto expense)
        {
            Expense expenseMapped = _mapper.Map<Expense>(expense);
            await _expenseRepository.AddExpense(expenseMapped);
        }

        public async Task UpdateExpense(ExpenseToUpdateDto expense, int userId)
        {
            if(expense.UserId != userId)
            {
                throw new Unauthorized(401, "Unauthorized Update");
            }
            Expense expenseMapped = _mapper.Map<Expense>(expense);
            
            await _expenseRepository.UpdateExpense(expenseMapped);
        }

        public async Task DeleteExpense(int expenseId, int userId)
        {
            
            Expense expense = await _expenseRepository.GetExpenseById(expenseId);

            if(expense.UserId != userId)
            {
                throw new Unauthorized(401, "Unauthorized operation");
            }
            
            await _expenseRepository.DeleteExpense(expenseId);
        }

    }
}