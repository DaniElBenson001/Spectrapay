using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spectrapay.Services.IServices;

namespace Spectrapay.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("all-transactions"), Authorize]
        public async Task<IActionResult> GetTransactionHistory()
        {
            var res = await _transactionService.GetTransactionHistory();
            return Ok(res);
        }
    }
}
