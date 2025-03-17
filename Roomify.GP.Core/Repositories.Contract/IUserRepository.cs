using Roomify.GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roomify.GP.Core.Repositories.Contract
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task<bool> UserExistsAsync(Guid? id);
        Task<User> GetUserByEmailAsync(string email);
   
    }
}
