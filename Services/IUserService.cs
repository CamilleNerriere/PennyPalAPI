using PennyPal.Dtos;
using PennyPal.Models;

namespace PennyPal.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers(int userId);
        Task<User?> GetUserById(int userId, int userConnectedId);
        Task UpdateUser(UserUpdateDto user, int userConnectedId);
        Task<Decimal> GetUserRemain(int userId);
    }
}