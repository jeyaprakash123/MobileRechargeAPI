using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TelecomProviderAPI.Application.Interfaces;

namespace TelecomProviderAPI.Controllers
{
    [Authorize]
    // Controllers/UserController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class MobileRechargeController : Controller
    {
        private readonly IMobileRechargeService _rechargeService;
        public MobileRechargeController(IMobileRechargeService rechargeService)
        {
            _rechargeService = rechargeService;
        }

        [HttpGet("topup-options")]
        public async Task<IActionResult> GetTopUpOptions()
        {
            var options = await _rechargeService.GetTopUpOptions();
            return Ok(options);
        }

        [HttpPost("top-up")]
        public async Task<IActionResult> TopUpBeneficiary(int userId, int beneficiaryId, decimal amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Amount must be greater than zero");
            }

            bool result = await _rechargeService.TopUpBeneficiary(userId, beneficiaryId, amount);
            return result ? Ok("Top-up successful") : BadRequest("Failed to complete top-up");
        }
    }
}
