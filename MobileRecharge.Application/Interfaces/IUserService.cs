using Microsoft.AspNetCore.Mvc;
using TopUpAPI.DataMapper;
using TopUpAPI.Models;

namespace TelecomProviderAPI.Application.Interfaces

{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<ActionResult<UserDto>> GetUser(int id);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(int id, bool isverified);

        Task<bool> DeleteUserAsync(int id);
    }
}
