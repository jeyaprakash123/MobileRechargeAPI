using TopUpAPI.DataMapper;
using AutoMapper;
using TelecomProviderAPI.Application.Interfaces;
using TelecomProviderAPI.Core.IRepository;
using Microsoft.Extensions.Configuration;
using MobileRecharge.Domain.UnitOfWork;

namespace TopUpAPI.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public BeneficiaryService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BeneficiaryDto>> GetBeneficiaries(int userId)
        {
            var res = await _unitOfWork.BeneficiaryRepository.GetBeneficiaries(userId);
            return _mapper.Map<IEnumerable<BeneficiaryDto>>(res);
        }

        public async Task<bool> AddBeneficiary(int userId, string nickname)
        {
            var res= await _unitOfWork.BeneficiaryRepository.AddBeneficiary(userId, nickname);
            if (res)
            {
                await _unitOfWork.CompleteAsync();
            }
            return res;
        }
        public async Task<bool> DeleteBeneficiary(int beneficiaryId)
        {
            var res = await _unitOfWork.BeneficiaryRepository.DeleteBeneficiary(beneficiaryId);
            if (res)
            {
                await _unitOfWork.CompleteAsync();
            }
            return res;
        }
    }
}
