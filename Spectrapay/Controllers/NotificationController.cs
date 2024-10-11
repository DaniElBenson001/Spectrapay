using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spectrapay.Services.IServices;

namespace Spectrapay.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("get-all-notifications"), Authorize]
        public async Task<IActionResult> GetAllNotifications()
        {
            var res = await _notificationService.GetAllNotifications();
            return Ok(res);
        }
    }
}
