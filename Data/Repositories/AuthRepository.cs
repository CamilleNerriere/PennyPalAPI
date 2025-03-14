using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PennyPal.Exceptions;
using PennyPal.Models;

namespace PennyPal.Data.Repositories
{
    public class AuthRepository : IAuthRepository

    {
        private readonly DataContextEF _entityFramework;

        public AuthRepository(DataContextEF entityFramework)
        {
            _entityFramework = entityFramework;
        }

        public async Task<Auth?> GetAuthByEmail(string Email)
        {
            return await _entityFramework.Auth.FindAsync(Email);
        }

        public async Task AddAuth(Auth auth)
        {
            if (auth == null)
            {
                throw new CustomValidationException("Auth object is null or empty");
            }
            await _entityFramework.Auth.AddAsync(auth);
        }

        public async Task UpdateAuth(Auth auth)
        {
            if (auth == null)
            {
                throw new CustomValidationException("Auth object is null or empty");
            }

            Auth? authToUpdate = await _entityFramework.Auth.FindAsync(auth.Email) ?? throw new NotFoundException("User not found");
            authToUpdate.PasswordHash = auth.PasswordHash ?? authToUpdate.PasswordHash;
            authToUpdate.PasswordSalt = auth.PasswordSalt ?? authToUpdate.PasswordSalt;

            await _entityFramework.SaveChangesAsync();
        }

        public async Task DeleteAuth(Auth auth)
        {
            Auth? authToDelete = await _entityFramework.Auth.FindAsync(auth.Email) ?? throw new NotFoundException("User not found");
            _entityFramework.Remove(authToDelete);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _entityFramework.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            await _entityFramework.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await _entityFramework.Database.RollbackTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _entityFramework.SaveChangesAsync();
        }
    }
}
