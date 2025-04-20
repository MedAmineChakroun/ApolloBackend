using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotification _notificationService;

        public NotificationController(INotification notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{tiersCode}")]
        public async Task<ActionResult<List<Notification>>> GetUserNotifications(string tiersCode)
        {
            var notifs = await _notificationService.GetUserNotifications(tiersCode);
            return Ok(notifs);
        }

        [HttpPost("read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsRead(id);
            return Ok(new { message = $"Notification {id} marked as read." });
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddNotification([FromBody] NotificationDto dto)
        {
            await _notificationService.AddNotificationAsync(dto.TiersCode, dto.Title, dto.Message ,dto.type);
            return Ok(new { message = "Notification created successfully." });
        }
        [HttpGet("unread/{tiersCode}")]
        public async Task<ActionResult<List<Notification>>> GetUserUnreadNotifications(string tiersCode)
        {
            var notifs = await _notificationService.getUserUnreadNotifications(tiersCode); 
            return Ok(notifs);
        }
    }

    public class NotificationDto
    {
        public string TiersCode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string type { get; set; }
    }

}
