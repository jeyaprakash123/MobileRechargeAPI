using BalanceApi.DataAccess;
using BalanceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BalanceApi.DataMapper;
using AutoMapper;

namespace BalanceApi.Services
{
    public class BalanceService:IBalanceService
    {
        private readonly BalanceDbContext _context;
        private readonly IMapper _mapper;
        public BalanceService(BalanceDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BalanceDto>> GetUser(int id)
        {
            var res= await _context.Balances
                .Where(u => u.UserId == id)
                .ToListAsync();
            return _mapper.Map<IEnumerable<BalanceDto>>(res);
        }
        public async Task<BalanceDto> CreateUserAsync(Balance balance)
        {
            //Add new user

            _context.Balances.Add(balance);
            await _context.SaveChangesAsync();
            return _mapper.Map<BalanceDto>(balance); ;
        }

        public async Task<bool> UpdateBalanceAsync(int id,decimal amount)
        {

            var user = await _context.Balances.FindAsync(id);
          
            if (user == null)
            {
                return false;
            }
            if (user.BalanceAmount < amount)
            {
                throw new InvalidOperationException("Insufficient balance.");
            }
            user.BalanceAmount-= amount;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
                {
                    return false;
                }
                else
                    throw;
            }
        }
        public async Task<bool> UserExists(int id)
        {
            return await _context.Balances.AnyAsync(e => e.Id == id);
        }

    }
}
