using AutoMapper;
using Roomify.GP.Core.DTOs.User;
using Roomify.GP.Core.Entities;
using Roomify.GP.Core.Repositories.Contract;
using Roomify.GP.Core.Services.Contract;
using Roomify.GP.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roomify.GP.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto> CreateUserAsync(UserCreateDto userDto)
        {
            // Check if email already exists
            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                // Return null or throw an exception 
                throw new Exception("This email is already registered.");
            }

            // Proceed to create user
            var user = _mapper.Map<User>(userDto);
            user.Password = PasswordHasher.HashPassword(user.Password);
            await _userRepository.AddUserAsync(user);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<bool> UpdateUserAsync(Guid id, UserUpdateDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;

            _mapper.Map(userDto, user);
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            if (!await _userRepository.UserExistsAsync(id)) return false;

            await _userRepository.DeleteUserAsync(id);
            return true;
        }
    }
}
