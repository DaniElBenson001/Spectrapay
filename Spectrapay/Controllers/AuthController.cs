using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spectrapay.Models.DtoModels;
using Spectrapay.Services.IServices;

namespace Spectrapay.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginUserDTO request)
        {
            var res = await _authService.Login(request);
            return Ok(res);
        }
    }
}
