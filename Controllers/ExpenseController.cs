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
    }
}