using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Tests.Services.User
{
    public class UserProfileServiceTests : TestBase
    {
        public UserProfileServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task TestGet_ReturnsSeededUser()
        {
            var user = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            Assert.NotNull(user);
            Assert.NotEqual(Guid.Empty, user.Id);
        }

        [Fact]
        public async Task TestGetByAuth()
        {
            var seededUser = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            Assert.NotNull(seededUser);

            var authenticated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetByAuthAsync(seededUser.UserName, seededUser.Password ?? "");
            });

            Assert.NotNull(authenticated);
            Assert.Equal(seededUser.Id, authenticated.Id);
        }

        [Fact]
        public async Task TestUpdate_ModifiesUserProfile()
        {
            var current = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            Assert.NotNull(current);

            var newUsername = "UpdatedAdmin_V2";
            var newPassword = "newPassword123";
            var updateDto = new UserProfileDto
            {
                Id = current.Id,
                UserName = newUsername,
                Password = newPassword,
                CurrencyId = CurrencyConstants.USD
            };

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                await service.UpdateAsync(updateDto);
            });

            var updated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            Assert.NotNull(updated);
            Assert.Equal(newUsername, updated.UserName);
            Assert.Equal(newPassword, updated.Password);
        }
    }
}
