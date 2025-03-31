using Microsoft.EntityFrameworkCore;
using PennyPal.Models;

namespace PennyPal.Data.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {

        private readonly DataContextEF _entityFramework;

        public RefreshTokenRepository(DataContextEF entityFramework)
        {
            _entityFramework = entityFramework;
        }

        public async Task AddToken(RefreshToken token)
        {
            await _entityFramework.RefreshTokens.AddAsync(token);
            await _entityFramework.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByToken(string token)
        {
            return await _entityFramework.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task InvalidateToken(RefreshToken token)
        {
            token.Revoked = true;
            await _entityFramework.SaveChangesAsync();
        }
    }
}