using Roomify.GP.Core.DTOs.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roomify.GP.Core.Services.Contract
{
    public interface IUserService
    {
        Task<UserResponseDto> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> CreateUserAsync(UserCreateDto userDto);
        Task<bool> UpdateUserAsync(Guid id, UserUpdateDto userDto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
