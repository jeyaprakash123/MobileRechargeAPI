using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MobileRecharge.Domain.UnitOfWork;
using System.Text;
using TelecomProviderAPI.Application.Interfaces;
using TelecomProviderAPI.Core.IRepository;
using TopUpAPI.DataMapper;
using TopUpAPI.Models;

namespace TelecomProviderAPI.Services
{
    public class MobileRechargeService: IMobileRechargeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public MobileRechargeService(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClientFactory.CreateClient("PaymentAPI");
            _config = config;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TopUpOptionDto>> GetTopUpOptions()
        {
            var res = await _unitOfWork.MobileRechargeRepository.GetTopUpOptions();
            return _mapper.Map<IEnumerable<TopUpOptionDto>>(res);
        }

        public async Task<bool> TopUpBeneficiary(int userId, int beneficiaryId, decimal amount)
        {
            if (amount <= 0)
            {
                throw new Exception("Invalid top-up amount.");
            }
            await _unitOfWork.MobileRechargeRepository.ValidatePlan(amount);

            var user = await _unitOfWork.MobileRechargeRepository.GetUser(userId);

            if (user == null) { throw new Exception("User not found."); }
           
            var beneficiary = user.Beneficiaries.FirstOrDefault(b => b.Id == beneficiaryId);
            if (beneficiary == null) { throw new Exception("Beneficiary not found."); }
            
            var totalTopUpsThisMonth = beneficiary.BeneficiaryTopUp
                .Where(x => x.MonthWise == DateTime.Now.Month && x.YearWise == DateTime.Now.Year)
                .Sum(x => x.Amount);

            var userTotalTopUpsThisMonth = _unitOfWork.MobileRechargeRepository.CheckUserMonthlyLimit(userId);

            if (!_unitOfWork.MobileRechargeRepository.UserTopUpLimitPerMonth(beneficiaryId, amount, totalTopUpsThisMonth))
                throw new Exception("User top-up Limit exceed for the beneficiary this month...Please wait until next month");

            // Update user's total top-up amount
            userTotalTopUpsThisMonth += amount;

            if (userTotalTopUpsThisMonth > user.TotalTopUpLimit)
                throw new Exception("User Monthly top-up limit exceeded for all beneficiaries.");

            var balance = await _unitOfWork.MobileRechargeRepository.GetUserBalance(userId);

            await _unitOfWork.MobileRechargeRepository.ValidateUserBalance(balance, amount);

            await _unitOfWork.MobileRechargeRepository.DoPayment(userId, amount);

            await _unitOfWork.MobileRechargeRepository.UpdateTransaction(userId, beneficiaryId, amount);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
