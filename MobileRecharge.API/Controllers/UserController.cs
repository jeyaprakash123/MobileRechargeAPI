using Microsoft.AspNetCore.Mvc;
using TopUpAPI.Models;
using TelecomProviderAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MobileRecharge.Domain.Configuration;
using MobileRecharge.Domain.Models;

namespace TopUpAPI.Controllers
{
    [Authorize]
    // Controllers/UserController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly Appsettings _appSettings;
        private readonly IConfiguration _config;
        private readonly decimal monthlyTopUpLimit;

        public UserController(IUserService userService, Appsettings appSettings)
        {
            _userService = userService;
            _appSettings = appSettings;
        }

        //Get:api/Users
        [HttpGet("get-all-users")] 
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("getuser")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var users = await _userService.GetUser(userId);

            if (users.Value == null)
            {
                return NotFound(new ResponseMessage { Message = "User Not Available"});
            }
            return Ok(users.Value);
        }

        [HttpPost("create-new-user")]

        public async Task<ActionResult<User>> CreateUser(string Name,bool isverified)
        {
            var user = new User
            {
                Username = Name,
                IsVerified = isverified,
                TotalTopUpLimit= _appSettings.UserMonthlyTopUpLimit,
            };
            var createUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createUser.Id }, createUser);
        }
        [HttpPut("userid")]
        public async Task<ActionResult<User>> UpdateUser(int id,bool isVerified)
        {
            var success = await _userService.UpdateUserAsync(id, isVerified);
            if (!success)
                return NotFound(new ResponseMessage { Message = "User Not Available" });
            return Ok(new ResponseMessage { Message = "User Details Successfully Updated" });
        }

        [HttpDelete("userid")]
        public async Task<ActionResult<User>> RemoveUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
                return NotFound(new ResponseMessage { Message = "User Not Available" });
            return Ok(new ResponseMessage {Message ="User Successfully removed"});
        }
    }
}
