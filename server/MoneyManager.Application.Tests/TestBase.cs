using MoneyManager.Tests.Shared.Fixtures;

namespace MoneyManager.Application.Tests
{
    public abstract class TestBase : MoneyManager.Tests.Shared.TestBase
    {
        protected TestBase(ServiceProviderFixture fixture) : base(fixture)
        {
        }
    }
}
