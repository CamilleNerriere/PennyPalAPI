using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PennyPal.Dtos;
using PennyPal.Helpers;
using PennyPal.Models;
using PennyPal.Services;

namespace PennyPal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController( IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            int userId = UserHelper.GetUserIdAsInt(User);
            IEnumerable<User> users =  await _userService.GetUsers(userId);
            return Ok(users);
        }

        [Authorize]
        [HttpGet("UserRemain")]
        public async Task<IActionResult> GetUserRemain()
        {
            int userId = UserHelper.GetUserIdAsInt(User);
            var remain = await _userService.GetUserRemain(userId);
            return Ok(remain);
        }

        [Authorize]
        [HttpGet("UserCategoriesRemain")]
        public async Task<IActionResult> GetUserCategoriesRemain()
        {
            int userId = UserHelper.GetUserIdAsInt(User);
            var categoriesRemains = await _userService.GetUserCategoryRemain(userId);
            return Ok(categoriesRemains);
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetUserById()
        {
            int userId = UserHelper.GetUserIdAsInt(User);
            int userConnectedId = UserHelper.GetUserIdAsInt(User);
            User? user = await _userService.GetUserById(userId, userConnectedId);
            return Ok(user);
        }

        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> UpdateUser(UserUpdateDto user)
        {
            int userId = UserHelper.GetUserIdAsInt(User);
            await _userService.UpdateUser(user, userId);
            return Ok();
        }

    }

}