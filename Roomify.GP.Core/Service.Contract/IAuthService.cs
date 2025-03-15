using Roomify.GP.Core.DTOs.User;
using Roomify.GP.Core.Entities;
using System.Threading.Tasks;

namespace Roomify.GP.Core.Service.Contract
{
    public interface IAuthService
    {
        Task<User?> VerifyGoogleTokenAsync(ExternalAuthDto externalAuth);
        Task ForgetPasswordAsync(ForgetPasswordRequest request);
        Task ResetPasswordAsync(ResetPasswordRequest request);

        string GenerateJwtToken(User user);
    }
}
