using AutoMapper;
using PennyPal.Data.Repositories;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Models;


namespace PennyPal.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private ILogger<ExpenseService> _logger;

        public ExpenseService(IExpenseRepository expenseRepository, ILogger<ExpenseService> logger)
        {
            _expenseRepository = expenseRepository;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ExpenseToAddDto, Expense>();
                cfg.CreateMap<ExpenseToUpdateDto, Expense>();
            }));
            _logger = logger;
        }

        public async Task<Expense> GetExpenseById(int userId, int expenseId)
        {
            var expense = await _expenseRepository.GetExpenseById(expenseId) ?? throw new NotFoundException("Expense not found");
            if (expense.UserId != userId)
            {
                _logger.LogError("Unauthorized attempt to access expense, userId : {userId}", userId);
                throw new Unauthorized(401, "Unauthorized action.");
            }

            return expense;
        }

        public async Task<IEnumerable<Expense>> GetExpensesByFilters(int userId, ExpenseFilterDto filters)
        {
            return await _expenseRepository.GetExpensesByFilters(userId, filters);
        }

        public async Task<decimal> GetExpensesTendances(int userId, ExpenseTendancesFilterDto filters)
        {
            IEnumerable<Expense> expenses = await _expenseRepository.GetExpensesTendances(userId, filters);

            decimal sum = 0;

            int numberOfExpenses = expenses.Count();

            static int GetMonthDifference(DateTime startDate, DateTime endDate)
            {
                return ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;
            }

            int nbMonth = 0;

            if (filters.Month1.HasValue && filters.Year1.HasValue && filters.Month2.HasValue && filters.Year2.HasValue)
            {
                var startDate = new DateTime(filters.Year1.Value, filters.Month1.Value, 1);
                var endDate = new DateTime(filters.Year2.Value, filters.Month2.Value, 1).AddMonths(1).AddDays(-1);
                nbMonth = GetMonthDifference(startDate, endDate) + 1;
            }

            foreach (Expense expense in expenses)
            {
                sum += expense.Amount;
            }

            if (numberOfExpenses != 0 && nbMonth != 0)
            {
                return Math.Round(sum / nbMonth, 2);
            }

            return 0;
        }

        public async Task AddExpense(ExpenseToAddDto expense)
        {
            var expenseMapped = _mapper.Map<Expense>(expense);
            await _expenseRepository.AddExpense(expenseMapped);
        }

        public async Task UpdateExpense(ExpenseToUpdateDto expense, int userId)
        {

            if (expense.UserId != userId)
            {
                _logger.LogError("Unauthorized attempt to update expense, userId : {userId}", userId);

                throw new Unauthorized(401, "Unauthorized Update");
            }
            var expenseMapped = _mapper.Map<Expense>(expense);

            await _expenseRepository.UpdateExpense(expenseMapped);
        }

        public async Task DeleteExpense(int expenseId, int userId)
        {

            var expense = await _expenseRepository.GetExpenseById(expenseId);

            if (expense.UserId != userId)
            {
                _logger.LogError("Unauthorized attempt to delete expense, userId : {userId}", userId);
                throw new Unauthorized(401, "Unauthorized operation");
            }

            await _expenseRepository.DeleteExpense(expenseId);
        }

        public async Task<List<Expense>> GetUserExpense(int userId)
        {
            return await _expenseRepository.GetUserExpense(userId);
        }

    }
}