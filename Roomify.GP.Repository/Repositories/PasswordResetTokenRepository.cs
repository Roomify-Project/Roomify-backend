using Microsoft.EntityFrameworkCore;
using Roomify.GP.Core.Entities;
using Roomify.GP.Core.Repositories;
using Roomify.GP.Repository.Data.Contexts;

namespace Roomify.GP.Repository.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly AppDbContext _context;

        public PasswordResetTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PasswordResetToken token)
        {
            await _context.PasswordResetTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResetToken?> GetByEmailAndTokenAsync(string email, string token)
        {
            return await _context.PasswordResetTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.User.Email == email && t.Token == token);
        }

        public async Task DeleteAsync(PasswordResetToken token)
        {
            _context.PasswordResetTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }
}
