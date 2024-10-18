using Microsoft.AspNetCore.Mvc;
using TopUpAPI.Models;
using TelecomProviderAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TopUpAPI.Controllers
{
    [Authorize]
    // Controllers/UserController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly decimal monthlyTopUpLimit;

        public UserController(IUserService userService,IConfiguration config)
        {
            _userService = userService;
            _config = config;
            monthlyTopUpLimit = _config.GetValue<decimal>("CustomSettings:UserMonthlyTopUpLimit");
        }

        //Get:api/Users
        [HttpGet("get-all-users")] //route name should be small
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
                return NotFound(new {message = "User Not Available"});
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
                TotalTopUpLimit=monthlyTopUpLimit,
            };
            var createUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createUser.Id }, createUser);
        }
        [HttpPut("userid")]
        public async Task<ActionResult<User>> UpdateUser(int id,bool isVerified)
        {
            var success = await _userService.UpdateUserAsync(id, isVerified);
            if (!success)
                return NotFound(new { message = "User Not Available" });
            return Ok(new { message = "User Details Successfully Updated" });
        }

        [HttpDelete("userid")]
        public async Task<ActionResult<User>> RemoveUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
                return NotFound(new { message = "User Not Available" });
            return Ok(new {message ="User Successfully removed"});
        }
    }
}
