using PennyPal.Dtos;

namespace PennyPal.Services
{
    public interface IAuthService
    {
        Task Register(UserForRegistrationDto user);
        Task<(string accessToken, string refreshToken, DateTime refreshExpiry)> Login(UserLoginDto user);
        Task<(string accessToken, string refreshToken, DateTime refreshExpiracy)> RefreshToken(string refreshToken);
        Task UpdatePassword(UserLoginDto user, int userId);
        Task DeleteAccount(int userId);

    }
}