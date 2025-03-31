using PennyPal.Models;

namespace PennyPal.Data.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddToken(RefreshToken token);
        Task<RefreshToken?> GetByToken(string token);
        Task InvalidateToken(RefreshToken token);
    }
}