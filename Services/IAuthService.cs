using PennyPal.Dtos;

namespace PennyPal.Services
{
    public interface IAuthService
    {
        Task Register(UserForRegistrationDto user);
        Task<Dictionary<string, string>> Login(UserLoginDto user);
        Task UpdatePassword(UserLoginDto user);
        Task DeleteAccount(string userId);

    }
}