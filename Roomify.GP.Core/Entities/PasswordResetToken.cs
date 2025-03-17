using System;

namespace Roomify.GP.Core.Entities
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }  // FK
        public string Token { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }

        public User User { get; set; } = null!;
    }
}
