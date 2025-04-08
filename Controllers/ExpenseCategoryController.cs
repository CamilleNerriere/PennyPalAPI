using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Helpers;
using PennyPal.Models;
using PennyPal.Services;

namespace PennyPal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ExpenseCategoryController : ControllerBase
    {
        private readonly IExpenseCategoryService _expenseCategoryService;
        private readonly ILogger<ExpenseCategoryController> _logger;

        public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService, ILogger<ExpenseCategoryController> logger)
        {
            _expenseCategoryService = expenseCategoryService;
            _logger = logger;
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetUserExpenseCategories()
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            IEnumerable<ExpenseCategoryDto> categories = await _expenseCategoryService.GetUserExpenseCategories(userId);
   
            return Ok(categories);
        }


        [HttpGet("MonthlyBudget")]
        [Authorize]
        public async Task<IActionResult> GetUserBudget()
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            IEnumerable<ExpenseCategoryDto> categories = await _expenseCategoryService.GetUserExpenseCategories(userId);

            decimal budget = 0m;

            foreach (var cat in categories)
            {
                budget += cat.MonthlyBudget;
            }

            return Ok(budget);
        }

        [HttpGet("{expenseCategoryId}")]
        [Authorize]
        public async Task<IActionResult> GetExpenseCategoryById(int expenseCategoryId)
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            ExpenseCategory? category = await _expenseCategoryService.GetExpenseCategoryById(expenseCategoryId, userId);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            return Ok(category);
        }
        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> AddExpenseCategory(ExpenseCategoryForRegistrationDto expenseCategory)
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            expenseCategory.UserId = userId;

            var category = await _expenseCategoryService.AddExpenseCategory(expenseCategory);

            return Ok(category);
        }

        [HttpPut()]
        [Authorize]
        public async Task<IActionResult> UpdateExpenseCategory(ExpenseCategoryForUpdateDto expenseCategory)
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            expenseCategory.UserId = userId;

            await _expenseCategoryService.UpdateExpenseCategory(expenseCategory, userId);

            return Ok();
        }

        [HttpDelete("{categoryId}")]
        [Authorize]
        public async Task<IActionResult> DeleteExpenseCategory(int categoryId)
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            await _expenseCategoryService.DeleteExpenseCategory(categoryId, userId);

            return Ok();
        }
    }
}