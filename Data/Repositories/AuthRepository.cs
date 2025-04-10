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

        public async Task AddAuth(Auth auth, CancellationToken cancellationToken = default)
        {
            if (auth == null)
            {
                throw new CustomValidationException("Auth object is null or empty");
            }
            await _entityFramework.Auth.AddAsync(auth, cancellationToken);
        }

        public async Task UpdateAuth(Auth auth)
        {
            if (auth == null)
            {
                throw new CustomValidationException("Auth object is null or empty");
            }

            var authToUpdate = await _entityFramework.Auth.FindAsync(auth.Email) ?? throw new NotFoundException("User not found");
            authToUpdate.PasswordHash = auth.PasswordHash ?? authToUpdate.PasswordHash;
            authToUpdate.PasswordSalt = auth.PasswordSalt ?? authToUpdate.PasswordSalt;

            await _entityFramework.SaveChangesAsync();
        }

        public async Task DeleteAuth(Auth auth)
        {
            var authToDelete = await _entityFramework.Auth.FindAsync(auth.Email) ?? throw new NotFoundException("User not found - AUTH");
            _entityFramework.Remove(authToDelete);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _entityFramework.Database.BeginTransactionAsync(cancellationToken);
        }
        public async Task CommitAsync()
        {
            await _entityFramework.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await _entityFramework.Database.RollbackTransactionAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
                await _entityFramework.SaveChangesAsync(cancellationToken);
        }

        public IExecutionStrategy GetExecutionStrategy()
        {
            return _entityFramework.Database.CreateExecutionStrategy();
        }
    }
}
