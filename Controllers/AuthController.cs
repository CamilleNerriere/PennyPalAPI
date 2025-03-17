using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Services;

namespace PennyPal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task Register(UserForRegistrationDto user)
        {
            await _authService.Register(user);
        }

        [HttpPost("Login")]
        public async Task<Dictionary<string, string>> Login(UserLoginDto user)
        {
            
            return await _authService.Login(user);
        }

        [HttpPut("ChangePassword")]
        public async Task UpdatePassword(UserLoginDto user)
        {
            await _authService.UpdatePassword(user);
        }

        [HttpDelete()]
        public async Task DeleteAccount()
        {
            Claim? userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new NotFoundException("User Not Found");
            }

            string userId = userIdClaim.Value;
            await _authService.DeleteAccount(userId);
            
        }

    }
}

