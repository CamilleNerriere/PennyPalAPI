using PennyPal.Dtos;
using PennyPal.Models;

namespace PennyPal.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User?> GetUserById(int userId);
        Task AddUser(UserDto user);
        Task UpdateUser(UserUpdateDto user);
        Task DeleteUser(int userId);
    }
}