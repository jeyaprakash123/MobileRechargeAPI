using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TelecomProviderAPI.Core.IRepository;
using TopUpAPI.DataAccess;
using TopUpAPI.Models;

namespace TelecomProviderAPI.Infrastructure.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly TopUpDbContext _context;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public UserRepository(TopUpDbContext context, IConfiguration config, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _config = config;
            _httpClient = httpClientFactory.CreateClient("PaymentApi");
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            //Fetch all user with their beneficiaries
           return await _context.Users.Include(u => u.Beneficiaries).ToListAsync();
           
        }
        public async Task<ActionResult<User>> GetUser(int id)
        {
            
            return await _context.Users.Include(u => u.Beneficiaries).FirstOrDefaultAsync(u => u.Id == id);
          
        }
        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            //await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(int id, bool isverified)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }
            user.IsVerified = isverified;
            try
            {
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
                {
                    return false;
                }
                else
                    throw ;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
           // await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UserExists(int id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }
    }
}
