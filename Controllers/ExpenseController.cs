using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Helpers;
using PennyPal.Models;
using PennyPal.Services;

namespace PennyPal.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [Authorize]
        [HttpGet("{expenseId}")]
        public async Task<IActionResult> GetExpenseById( int expenseId)
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            Expense expense = await _expenseService.GetExpenseById(userId, expenseId);

            return Ok(expense);

        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetExpensesByFilters([FromQuery] ExpenseFilterDto filters)
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            IEnumerable<Expense> expenses = await _expenseService.GetExpensesByFilters(userId, filters);

            return Ok(expenses);
        }

        [Authorize]
        [HttpGet("Tendances")]
        public async Task<IActionResult> GetExpensesTendances([FromQuery] ExpenseTendancesFilterDto filters)
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            decimal expenses = await _expenseService.GetExpensesTendances(userId, filters);

            return Ok(expenses);
        }
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> AddExpense(ExpenseToAddDto expense)
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            expense.UserId = userId;

            await _expenseService.AddExpense(expense);

            return Ok();
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateExpense(ExpenseToUpdateDto expense)
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            await _expenseService.UpdateExpense(expense, userId);

            return Ok();
        }

        [Authorize]
        [HttpDelete("Delete/{expenseId}")]
        public async Task<IActionResult> DeleteExpense(int expenseId)
        {
            int userId = UserHelper.GetUserIdAsInt(User);

            await _expenseService.DeleteExpense(expenseId, userId);

            return Ok();
        }
    }
}