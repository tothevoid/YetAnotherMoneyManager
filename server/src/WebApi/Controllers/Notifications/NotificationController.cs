using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Common;
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

        [HttpPost(nameof(GetAll))]
        public async Task<IEnumerable<NotificationModel>> GetAll(GetAllNotificationsQuery query)
        {
            var notifications = await _notificationService.GetAllAsync(query.PageIndex, query.RecordsQuantity, query.OnlyUnread, query.Category);
            return _mapper.Map(notifications);
        }

        [HttpGet(nameof(GetPagination))]
        public async Task<PaginationConfigModel> GetPagination(
            [FromQuery] bool onlyUnread = false,
            [FromQuery] string category = null)
        {
            var pagination = await _notificationService.GetPaginationAsync(onlyUnread, category);
            return _mapper.Map(pagination);
        }

        [HttpGet("unread-count")]
        public async Task<int> GetUnreadCount()
        {
            return await _notificationService.GetUnreadCountAsync();
        }

        [HttpPost("{id}/read")]
        public async Task MarkAsRead(Guid id)
        {
            await _notificationService.MarkAsReadAsync(id);
        }

        [HttpPost("read-all")]
        public async Task MarkAllAsRead()
        {
            await _notificationService.MarkAllAsReadAsync();
        }

        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _notificationService.DeleteAsync(id);
        }
    }
}
