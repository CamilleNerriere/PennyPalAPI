using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PennyPal.Exceptions;

namespace PennyPal.Helpers
{
    public static class UserHelper
    {
         public static string GetUserId(ClaimsPrincipal user)
        {
            Claim? userIdClaim = user.FindFirst("userId");
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new NotFoundException("User Not Found");
            }

            return userIdClaim.Value;
        }

        public static int GetUserIdAsInt(ClaimsPrincipal user)
        {
            string userId = GetUserId(user);
            
            if (!int.TryParse(userId, out int userIdInt))
            {
                throw new CustomValidationException("UserId format invalide");
            }
            
            return userIdInt;
        }
    }
}