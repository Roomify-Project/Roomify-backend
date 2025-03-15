using Roomify.GP.Core.Entities;

namespace Roomify.GP.Core.Repositories
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetByEmailAndTokenAsync(string email, string token);
        Task DeleteAsync(PasswordResetToken token);
    }
}
