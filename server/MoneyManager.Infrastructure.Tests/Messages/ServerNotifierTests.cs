using Microsoft.AspNetCore.SignalR;
using MoneyManager.Infrastructure.Messages;
using NSubstitute;
using Xunit;

namespace MoneyManager.Infrastructure.Tests.Messages
{
    public class ServerNotifierTests
    {
        [Fact]
        public async Task SendToAll_CallsSignalRHubClientsAll()
        {
            var mockHubContext = Substitute.For<IHubContext<ServerMessagesHub>>();
            var mockClients = Substitute.For<IHubClients>();
            var mockClientProxy = Substitute.For<IClientProxy>();

            mockHubContext.Clients.Returns(mockClients);
            mockClients.All.Returns(mockClientProxy);

            var notifier = new ServerNotifier(mockHubContext);

            await notifier.SendToAll("Test Message");

            await mockClientProxy.Received(1).SendCoreAsync(
                "ReceiveServerMessage",
                Arg.Is<object?[]>(args => args.Length == 1 && (string?)args[0] == "Test Message"),
                Arg.Any<CancellationToken>()
            );
        }
    }
}
