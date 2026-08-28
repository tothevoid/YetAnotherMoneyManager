using Audex.WebApi.Models.Common;

namespace Audex.WebApi.Models.Notifications
{
    public class GetAllNotificationsQuery : BasePageableQuery
    {
        public bool OnlyUnread { get; set; }
        public string Category { get; set; }
    }
}
