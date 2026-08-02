using MoneyManager.Application.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace MoneyManager.Application.Tests
{
    public abstract class TestBase : IClassFixture<ServiceCollectionFixture>
    {
        protected readonly ServiceCollectionFixture Fixture;

        protected TestBase(ServiceCollectionFixture fixture)
        {
            Fixture = fixture;
        }

        protected async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using var scope = Fixture.ServiceProvider.CreateScope();
            await action(scope.ServiceProvider);
        }

        protected async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using var scope = Fixture.ServiceProvider.CreateScope();
            return await action(scope.ServiceProvider);
        }
    }
}
