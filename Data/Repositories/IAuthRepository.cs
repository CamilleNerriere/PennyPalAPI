using Microsoft.EntityFrameworkCore.Storage;
using PennyPal.Models;

namespace PennyPal.Data.Repositories
{
    public interface IAuthRepository
    {
        Task<Auth?> GetAuthByEmail(string Email);
        Task AddAuth(Auth auth);
        Task UpdateAuth(Auth auth);
        Task DeleteAuth(Auth auth);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync();
    }

}