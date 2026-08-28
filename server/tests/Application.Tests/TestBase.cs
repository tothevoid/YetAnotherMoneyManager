using Audex.Tests.Shared.Fixtures;

namespace Audex.Application.Tests
{
    public abstract class TestBase : Audex.Tests.Shared.TestBase
    {
        protected TestBase(ServiceProviderFixture fixture) : base(fixture)
        {
        }
    }
}
