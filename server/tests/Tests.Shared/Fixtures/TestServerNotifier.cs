using Audex.Infrastructure.Interfaces.Messages;

namespace Audex.Tests.Shared.Fixtures
{
    public class TestServerNotifier : IServerNotifier
    {
        public Task SendToAllAsync(string? message = null)
        {
            return Task.CompletedTask;
        }
    }
}
