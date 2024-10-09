using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spectrapay.Models.DtoModels;
using Spectrapay.Services.IServices;

namespace Spectrapay.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPut("transfer"), Authorize]
        public async Task<IActionResult> makeTransaction(PaymentDTO transfer)
        {
            var res = await _paymentService.MakeTransaction(transfer);
            return Ok(res);
        }
    }
}
