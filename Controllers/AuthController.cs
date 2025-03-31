using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Helpers;
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
        public async Task<IActionResult> Register(UserForRegistrationDto user)
        {
            await _authService.Register(user);
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            var (accessToken, refreshToken, refreshExpiry) = await _authService.Login(user);
            HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshExpiry
            });

            return Ok(new {token = accessToken});
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            
            if(string.IsNullOrEmpty(refreshToken))
            {
                throw new Unauthorized(401, "Invalid Token");
            }

            var(newAccessToken, newRefreshToken, refreshExpiry) = await _authService.RefreshToken(refreshToken);

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true, 
                Secure= true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshExpiry
            }
            );

            return Ok(new {token = newAccessToken});
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> UpdatePassword(UserLoginDto user)
        {
            int userConnectedId = UserHelper.GetUserIdAsInt(User);
            await _authService.UpdatePassword(user, userConnectedId);
            return Ok();
        }

        [Authorize]
        [HttpDelete()]
        public async Task<IActionResult> DeleteAccount()
        {
            int userId = UserHelper.GetUserIdAsInt(User);
            await _authService.DeleteAccount(userId);
            return Ok();
        }

    }
}

