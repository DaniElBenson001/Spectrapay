using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spectrapay.Models.DtoModels;
using Spectrapay.Services.IServices;

namespace Spectrapay.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(CreateUserDTO request)
        {
            var res = await _userService.CreateUser(request);
            return Ok(res);
        }

    }
}
