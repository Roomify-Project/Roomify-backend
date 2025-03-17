using Roomify.GP.Core.Enums;
using System;

namespace Roomify.GP.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public Roles Roles { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Google Auth
        public string? Provider { get; set; }
        public string? ProviderId { get; set; }

    }
}
