using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Roomify.GP.Core.DTOs.User;
using Roomify.GP.Core.Entities;
using Roomify.GP.Core.Repositories.Contract;
using Roomify.GP.Core.Service.Contract;

namespace Roomify.GP.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IConfiguration configuration, IUserRepository userRepository, IJwtService jwtService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<User?> VerifyGoogleTokenAsync(ExternalAuthDto externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { _configuration["GoogleAuthSettings:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);

                if (payload == null) return null;

                var user = await _userRepository.GetUserByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new User
                    {
                        Email = payload.Email,
                        FullName = payload.Name,
                        Provider = "Google",
                        ProviderId = payload.Subject,
                        PictureUrl = payload.Picture
                    };

                    await _userRepository.AddUserAsync(user);
                }

                return user;
            }
            catch
            {
                return null;
            }
        }

        public string GenerateJwtToken(User user)
        {
            return _jwtService.GenerateToken(user);
        }




    }
}
