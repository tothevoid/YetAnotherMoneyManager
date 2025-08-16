using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MoneyManager.Infrastructure.Interfaces.Messages;

namespace MoneyManager.Infrastructure.Messages
{
    public class ServerNotifier: IServerNotifier
    {
        private readonly IHubContext<ServerMessagesHub> _hubContext;
        public ServerNotifier(IHubContext<ServerMessagesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToAll(string message = null)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveServerMessage", message);
        }
    }
}
