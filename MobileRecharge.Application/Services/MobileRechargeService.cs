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
            await _unitOfWork.MobileRechargeRepository.TopUpBeneficiary(userId, beneficiaryId, amount);

            var balance = await _unitOfWork.MobileRechargeRepository.GetUserBalance(userId);

            await _unitOfWork.MobileRechargeRepository.ValidateUserBalance(balance, amount);

            await _unitOfWork.MobileRechargeRepository.DoPayment(userId, amount);

            await _unitOfWork.MobileRechargeRepository.UpdateTransaction(userId, beneficiaryId, amount);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
