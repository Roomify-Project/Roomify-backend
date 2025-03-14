﻿using Microsoft.AspNetCore.Mvc;
using Roomify.GP.Core.DTOs.User;
using Roomify.GP.Core.Repositories.Contract;
using Roomify.GP.Core.Services.Contract;
using Roomify.GP.Service.Helpers;
using System.Threading.Tasks;
using Roomify.GP.Core.DTOs.User;

using Roomify.GP.Core.Service.Contract;



namespace Roomify.GP.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IUserRepository userRepository, IJwtService jwtService, IAuthService authService)
        {
            _userService = userService;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _authService = authService;
        }



        public AuthController(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto userDto)
        {
            var user = await _userService.CreateUserAsync(userDto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);

            if (user == null || user.Password != PasswordHasher.HashPassword(loginDto.Password))
                return Unauthorized("Invalid email or password");

            var token = _jwtService.GenerateToken(user);

            var response = new LoginResponseDto

            {
                Token = token,
                UserName = user.UserName,
                Role = user.Role
            };

            return Ok(response);
        }
      
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] ExternalAuthDto externalAuth)
        {
            if (externalAuth == null || externalAuth.Provider != "Google")
                return BadRequest("Invalid request");

            var user = await _authService.VerifyGoogleTokenAsync(externalAuth);
            if (user == null)
                return Unauthorized("Invalid Google Token");

            var token = _authService.GenerateJwtToken(user);

            var response = new LoginResponseDto
            {
                UserName = user.UserName,
                Role = user.Role,
                Token = token
            };

            return Ok(response);
        }

    }
}
