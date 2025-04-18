using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using PennyPal.Dtos;
using PennyPal.Exceptions;
using PennyPal.Models;

namespace PennyPal.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContextEF _entityFramework;

        public UserRepository(DataContextEF entityFramework)
        {
            _entityFramework = entityFramework;

        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _entityFramework.User.ToListAsync<User>();
        }

        public async Task<User?> GetUserById(int userId)
        {
            var user = await _entityFramework.User.FindAsync(userId) ?? throw new NotFoundException($"User {userId} not found");

            return user;

        }

        public async Task<User?> GetUserByEmail(UserDto userDto)
        {
            return await _entityFramework.User
                .FirstOrDefaultAsync(e => e.Email == userDto.Email);

        }

        public async Task AddUser(User user, CancellationToken cancellationToken = default)
        {
            if (user == null)
            {
                throw new CustomValidationException("User object is null or empty");
            }

            await _entityFramework.User.AddAsync(user, cancellationToken);

        }

        public async Task UpdateUser(UserUpdateDto user)
        {
            if (user == null)
            {
                throw new CustomValidationException("User object is null or emptuy");
            }
            var userToUpdate = await _entityFramework.User.FindAsync(user.Id) ?? throw new NotFoundException("User not Found");
            userToUpdate.Firstname = user.Firstname ?? userToUpdate.Firstname;
            userToUpdate.Lastname = user.Lastname ?? userToUpdate.Lastname;

            await _entityFramework.SaveChangesAsync();

        }

        public async Task DeleteUser(int userId)
        {
            var user = await _entityFramework.User.FindAsync(userId) ?? throw new NotFoundException("User not found");

            _entityFramework.Remove(user);

        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _entityFramework.SaveChangesAsync(cancellationToken);
        }

    }
}

