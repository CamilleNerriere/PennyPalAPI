using AutoMapper;
using PennyPal.Data.Repositories;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Models;

namespace PennyPal.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IExpenseService _expenseService;
        private readonly IExpenseCategoryService _expenseCategoryService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IAuthRepository authRepository, IExpenseCategoryService expenseCategoryService, IExpenseService expenseService)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
            _expenseCategoryService = expenseCategoryService;
            _expenseService = expenseService;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDto, User>();
            }));
        }

        public async Task<User?> GetUserById(int userId, int userConnectedId)
        {
            User user = await _userRepository.GetUserById(userId) ?? throw new NotFoundException("User not found");

            if (user.Id != userConnectedId)
            {
                throw new Unauthorized(401, "Unauthorized Operation");
            }

            return user;
        }

        public async Task UpdateUser(UserUpdateDto user, int userConnectedId)
        {
            if(user.Id != userConnectedId)
            {
                throw new Unauthorized(401, "Unauthorized Operation");
            }
            await _userRepository.UpdateUser(user);
        }

        public async Task<Decimal> GetUserRemain(int userId)
        {

            IEnumerable<ExpenseCategoryDto> categories = await _expenseCategoryService.GetUserExpenseCategories(userId);

            var budget = 0m;

            foreach (var cat in categories)
            {
                budget += cat.MonthlyBudget;
            }

            List<Expense> userExpenses = await _expenseService.GetUserExpense(userId);

            var expenses = 0m;

            var date = DateTime.Now;

            foreach (var exp in userExpenses)
            {
                if (exp.Date.Month == date.Month)
                {
                    expenses += exp.Amount;
                }
            }

            return budget - expenses;
        }

        public async Task<IEnumerable<ExpenseCategoryRemainDto>> GetUserCategoryRemain(int userId)
        {
            IEnumerable<ExpenseCategoryDto> categories = await _expenseCategoryService.GetUserExpenseCategories(userId);

            List<ExpenseCategoryRemainDto> categoryRemains = []; 

            var date = DateTime.Now;

            foreach (var cat in categories)
            {
                decimal budget = cat.MonthlyBudget;

                foreach (var exp in cat.Expenses)
                {
                    if(exp.Date.Month == date.Month)
                    {
                        budget -= exp.Amount;
                    }
                }

                ExpenseCategoryRemainDto category = new()
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    MonthlyBudget = cat.MonthlyBudget,
                    Remain = budget
                }; 

                categoryRemains.Add(category);
            }

            return categoryRemains;
        }

    }
}