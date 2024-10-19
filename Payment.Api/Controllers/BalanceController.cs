using Microsoft.AspNetCore.Mvc;
using BalanceApi.Services;
using BalanceApi.Models;
using Payment.Api.Models;

namespace BalanceApi.Controllers
{
    // Controllers/UserController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : Controller
    {
        private readonly IBalanceService _balanceService;
        public BalanceController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [HttpGet("get-user-balance")]
        public async Task<ActionResult<Balance>> GetUser([FromQuery] int UserId)
        {
            var users = await _balanceService.GetUser(UserId);

            if (users == null)
            {
                return NotFound(new ResponseMessage { Message = "User details not available" });
            }
            return Ok(users.FirstOrDefault().BalanceAmount);
        }

        [HttpPost("add-balance")]

        public async Task<ActionResult<Balance>> AddBalance(int userid, decimal amount)
        {
            var balance = new Balance
            {
                UserId = userid,
                BalanceAmount = amount
            };
            var createUser = await _balanceService.CreateUserAsync(balance);
            return CreatedAtAction(nameof(GetUser), new { id = createUser.Id }, createUser);
        }

        [HttpPut("make-payment")]

        public async Task<ActionResult<Balance>> MakePayment(int userid, [FromBody] PaymentRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ResponseMessage { Message = "Invalid payment request" });
            }

            var success = await _balanceService.UpdateBalanceAsync(userid,request.TotalAmount);
            if (!success)
                return NotFound(new ResponseMessage { Message = "User Not Available" });
            return Ok(new ResponseMessage { Message = "User Balance Details Successfully Updated" });
        }
    }
}