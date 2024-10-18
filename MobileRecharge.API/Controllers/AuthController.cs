using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileRecharge.Domain.Models;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TopUpAPI.Models;
using TopUpAPI.DataAccess;
using Microsoft.AspNetCore.Identity;

namespace MobileRecharge.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TopUpDbContext _context;

        public UserController(IConfiguration configuration, TopUpDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(string userName, string password)
        {
            // Validate the user credentials

            var user = _context.UserLogin.SingleOrDefault(u => u.Username == userName);
            if (user == null)
            {
                return Unauthorized();
            }

            // Verify password (assuming you hash passwords)
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return Unauthorized(); // Invalid password
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }
    }

}
