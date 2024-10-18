using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TelecomProviderAPI.Application.Interfaces;
using TelecomProviderAPI.Core.IRepository;
using TopUpAPI.DataMapper;
using TopUpAPI.Models;
using MobileRecharge.Domain.UnitOfWork;

namespace TopUpAPI.Services
{
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IConfiguration config,IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _httpClient = httpClientFactory.CreateClient("PaymentAPI");
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            //Fetch all user with their beneficiaries
            var users= await _unitOfWork.UserRepository.GetUsersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var users= await _unitOfWork.UserRepository.GetUser(id);
            return _mapper.Map<UserDto>(users.Value);
        }
        public async Task<User> CreateUserAsync(User user)
        {
            user.AvailableBalance = await _unitOfWork.MobileRechargeRepository.GetUserBalance(user.Id);
            var result= await _unitOfWork.UserRepository.CreateUserAsync(user);
            
            await _unitOfWork.CompleteAsync();
            return result;
        }

        public async Task<bool> UpdateUserAsync(int id,bool isverified)
        {
            var res= await _unitOfWork.UserRepository.UpdateUserAsync(id, isverified);
            if(res)
            {
                await _unitOfWork.CompleteAsync();
            }
            return res;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var result = await _unitOfWork.UserRepository.DeleteUserAsync(id);
            if(result)
            {
                await _unitOfWork.CompleteAsync();
            }
            return result;
        }
    }
}
