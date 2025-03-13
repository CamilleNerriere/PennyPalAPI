using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
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
            User? user =  await _entityFramework.User.FindAsync(userId);

            if (user != null)
            {
                return user;
            }
            else 
            {
                throw new NotFoundException($"User {userId} not found");
            }
        }

        public async Task AddUser(User user)
        {
            if (user != null)
            {
                await _entityFramework.AddAsync(user);
            }

            throw new CustomValidationException("Please, add some user informations.");
        }

        public async Task UpdateUser(User user)
        {
            if (user != null)
            {
                User? userToUpdate = await _entityFramework.User.FindAsync(user.Id);

                if (userToUpdate != null)
                {
                    userToUpdate.Firstname = user.Firstname ?? userToUpdate.Firstname;
                    userToUpdate.Lastname = user.Lastname ?? userToUpdate.Lastname;
                    userToUpdate.Email = user.Email ?? userToUpdate.Email;
                    await _entityFramework.SaveChangesAsync();
                }
                throw new Exception("Unable to Update");
            }
            throw new NotFoundException("User not found.");
        }

        public async Task DeleteUser(int userId)
        {
            User? user = await _entityFramework.User.FindAsync(userId);

            if(user != null)
            {
                _entityFramework.Remove(user);
                await _entityFramework.SaveChangesAsync();
            }
            throw new NotFoundException("User not found");
        }

    }
}

