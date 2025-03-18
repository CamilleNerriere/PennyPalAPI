using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PennyPal.Dtos;
using PennyPal.Exceptions;
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
        [HttpGet()]
        public async Task<IActionResult> GetExpensesByFilters([FromQuery] ExpenseFilterDto filters)
        {
            Claim? userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new NotFoundException("User Not Found");
            }

            int userId = Int32.Parse(userIdClaim.Value);

            IEnumerable<Expense> expenses = await _expenseService.GetExpensesByFilters(userId, filters);

            return Ok(expenses);
        }

        [Authorize]
        [HttpGet("Tendances")]
        public async Task<IActionResult> GetExpensesTendances([FromQuery] ExpenseTendancesFilterDto filters)
        {
            Claim? userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new NotFoundException("User Not Found");
            }

            int userId = Int32.Parse(userIdClaim.Value);

            decimal expenses = await _expenseService.GetExpensesTendances(userId, filters);

            return Ok(expenses);
        }
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> AddExpense(ExpenseToAddDto expense)
        {
            Claim? userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new NotFoundException("User Not Found");
            }

            int userId = Int32.Parse(userIdClaim.Value);

            expense.UserId = userId;

            await _expenseService.AddExpense(expense);

            return Ok();
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateExpense(ExpenseToUpdateDto expense)
        {
            Claim? userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new NotFoundException("User Not Found");
            }

            int userId = Int32.Parse(userIdClaim.Value);

            await _expenseService.UpdateExpense(expense, userId);

            return Ok();
        }

        [Authorize]
        [HttpDelete("Delete/{expenseId}")]
        public async Task<IActionResult> DeleteExpense(int expenseId)
        {
            Claim? userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new NotFoundException("User Not Found");
            }

            int userId = Int32.Parse(userIdClaim.Value);

            await _expenseService.DeleteExpense(expenseId, userId);

            return Ok();
        }
    }
}