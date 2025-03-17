using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Roomify.GP.Core.DTOs.User;
using Roomify.GP.Core.Entities;
using Roomify.GP.Core.Repositories;
using Roomify.GP.Core.Repositories.Contract;
using Roomify.GP.Core.Service.Contract;
using Roomify.GP.Service.Helpers;
using System.Web;

namespace Roomify.GP.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IEmailService _emailService;


        public AuthService(IConfiguration configuration, IUserRepository userRepository, IJwtService jwtService, IEmailService emailService ,IPasswordResetTokenRepository passwordResetTokenRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _emailService = emailService;
            _passwordResetTokenRepository = passwordResetTokenRepository;
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
                        ProfilePicture = payload.Picture
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

        public async Task ForgetPasswordAsync(ForgetPasswordRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("User not found");

            // توليد Token وتخزينه
            var resetToken = Guid.NewGuid().ToString();
            var expiration = DateTime.UtcNow.AddHours(1); // صلاحية التوكن ساعة واحدة

            var tokenEntity = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = resetToken,
                ExpirationDate = expiration
            };

            await _passwordResetTokenRepository.AddAsync(tokenEntity);

            // بناء رابط Reset
            var baseUrl = _configuration["AppSettings:FrontendBaseUrl"];
            var resetLink = $"{baseUrl}/reset-password?token={HttpUtility.UrlEncode(resetToken)}&email={HttpUtility.UrlEncode(request.Email)}";

            // محتوى الإيميل
            var body = $"<h3>Reset Your Password</h3><p>Click the link below:</p><a href='{resetLink}'>Reset Password</a>";

            await _emailService.SendEmailAsync(request.Email, "Reset Your Password", body);
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            var tokenRecord = await _passwordResetTokenRepository.GetByEmailAndTokenAsync(request.Email, request.Token);

            if (tokenRecord == null || tokenRecord.ExpirationDate < DateTime.UtcNow)
                throw new Exception("Invalid or expired token");

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("User not found");

            user.Password = PasswordHasher.HashPassword(request.NewPassword);
            await _userRepository.UpdateUserAsync(user);

            await _passwordResetTokenRepository.DeleteAsync(tokenRecord);
        }


    }
}
