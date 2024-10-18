using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUpAPI.Models;

namespace TelecomProviderAPI.Core.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<ActionResult<User>> GetUser(int id);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(int id, bool isverified);

        Task<bool> DeleteUserAsync(int id);
        Task<bool> UserExists(int id);
    }
}