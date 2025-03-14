using PennyPal.Dtos;
using PennyPal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PennyPal.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User?> GetUserById(int userId);
        Task<User?> GetUserByEmail(UserDto userDto);
        Task AddUser(User user);
        Task UpdateUser(UserUpdateDto user);
        Task DeleteUser(int userId);
        Task SaveChangesAsync();

    }
}
