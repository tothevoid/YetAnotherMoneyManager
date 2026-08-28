using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Audex.Infrastructure.Interfaces.Messages;

namespace Audex.Infrastructure.Messages
{
    public class ServerNotifier: IServerNotifier
    {
        private readonly IHubContext<ServerMessagesHub> _hubContext;
        public ServerNotifier(IHubContext<ServerMessagesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToAllAsync(string message = null)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveServerMessage", message);
        }
    }
}
