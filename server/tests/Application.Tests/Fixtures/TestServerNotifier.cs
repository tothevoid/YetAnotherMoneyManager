using MoneyManager.Infrastructure.Interfaces.Messages;

namespace MoneyManager.Application.Tests.Fixtures
{
    public class TestServerNotifier : IServerNotifier
    {
        public Task SendToAllAsync(string? message = null)
        {
            return Task.CompletedTask;
        }
    }
}
