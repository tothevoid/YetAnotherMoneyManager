using Audex.Infrastructure.Interfaces.Messages;

namespace Audex.Application.Tests.Fixtures
{
    public class TestServerNotifier : IServerNotifier
    {
        public Task SendToAllAsync(string? message = null)
        {
            return Task.CompletedTask;
        }
    }
}
