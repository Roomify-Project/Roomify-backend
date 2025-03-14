using Roomify.GP.Core.DTOs.User;
using Roomify.GP.Core.Entities;
using System.Threading.Tasks;

namespace Roomify.GP.Core.Service.Contract
{
    public interface IAuthService
    {
        Task<User?> VerifyGoogleTokenAsync(ExternalAuthDto externalAuth);
        string GenerateJwtToken(User user);
    }
}
