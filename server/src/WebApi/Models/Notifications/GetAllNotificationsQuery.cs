using MoneyManager.WebApi.Models.Common;

namespace MoneyManager.WebApi.Models.Notifications
{
    public class GetAllNotificationsQuery : BasePageableQuery
    {
        public bool OnlyUnread { get; set; }
        public string Category { get; set; }
    }
}
