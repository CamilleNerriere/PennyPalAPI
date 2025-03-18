using PennyPal.Dtos;

namespace PennyPal.Services
{
    public interface IAuthService
    {
        Task Register(UserForRegistrationDto user);
        Task<Dictionary<string, string>> Login(UserLoginDto user);
        Task UpdatePassword(UserLoginDto user, int userId);
        Task DeleteAccount(int userId);

    }
}