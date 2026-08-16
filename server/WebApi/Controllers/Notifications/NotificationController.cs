using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Notifications;

namespace MoneyManager.WebApi.Controllers.Notifications
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly WebApiMapper _mapper;

        public NotificationController(INotificationService notificationService, WebApiMapper mapper)
        {
            _notificationService = notificationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<NotificationModel>> GetAll([FromQuery] bool? onlyUnread = null)
        {
            var notifications = await _notificationService.GetAll(onlyUnread);
            return _mapper.Map(notifications);
        }

        [HttpGet("unread-count")]
        public async Task<int> GetUnreadCount()
        {
            return await _notificationService.GetUnreadCount();
        }

        [HttpPost("{id}/read")]
        public async Task MarkAsRead(Guid id)
        {
            await _notificationService.MarkAsRead(id);
        }

        [HttpPost("read-all")]
        public async Task MarkAllAsRead()
        {
            await _notificationService.MarkAllAsRead();
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _notificationService.Delete(id);
        }
    }
}
