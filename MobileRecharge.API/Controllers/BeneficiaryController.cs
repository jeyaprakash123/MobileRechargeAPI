using Microsoft.AspNetCore.Mvc;
using Serilog;
using TopUpAPI.Models;
using TelecomProviderAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TopUpAPI.Controllers
{
    [Authorize]
    // Controllers/TopUpController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiaryController : ControllerBase
    {
        private readonly IBeneficiaryService _topUpService;

        public BeneficiaryController(IBeneficiaryService topUpService)
        {
            _topUpService = topUpService;
        }

        [HttpGet("beneficiaries")]
        public async Task<IActionResult> GetBeneficiaries(int userId)
        {

            var beneficiaries = await _topUpService.GetBeneficiaries(userId);
            return Ok(beneficiaries);

        }

        [HttpPost("add-beneficiary")]

        public async Task<IActionResult> AddBeneficiary(int userId, string nickname)
        {

            bool result = await _topUpService.AddBeneficiary(userId, nickname);
            return result ? Ok("Beneficiary added successfully") : BadRequest("Failed to add beneficiary");

        }

        [HttpDelete("beneficiaryid")]
        public async Task<ActionResult<Beneficiary>> RemoveBeneficiary(int beneficiaryId)
        {
            var success = await _topUpService.DeleteBeneficiary(beneficiaryId);
            if (!success)
                return NotFound(new { message = "Beneficiay Not Available" });
            return Ok(new { message = "Beneficiary removed Successfully" });
        }

    }


}
