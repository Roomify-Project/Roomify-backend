using Microsoft.EntityFrameworkCore;
using Roomify.GP.Core.Entities;

namespace Roomify.GP.Repository.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<PortfolioPost> PortfolioPosts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
