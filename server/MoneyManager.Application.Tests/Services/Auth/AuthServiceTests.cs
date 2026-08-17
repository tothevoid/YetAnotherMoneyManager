using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Application.Services.Auth;
using MoneyManager.Application.Tests.Fixtures;

namespace MoneyManager.Application.Tests.Services.Auth
{
    public class AuthServiceTests : TestBase
    {
        public AuthServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestLoginAndChangePassword()
        {
            var user = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            Assert.NotNull(user);

            // Test Login
            var token = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "");
            });

            Assert.False(string.IsNullOrWhiteSpace(token));

            // Test ChangePassword
            var newPassword = "newPassword_" + Guid.NewGuid().ToString("N");

            var changeResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.ChangePasswordAsync(user.UserName, user.Password ?? "", newPassword);
            });

            Assert.True(changeResult);

            // Test Login with new password
            var newToken = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, newPassword);
            });

            Assert.False(string.IsNullOrWhiteSpace(newToken));
        }

        [Fact]
        public async Task TestLogin_WithInvalidCredentials_ThrowsException()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await ExecuteScopeAsync(async sp =>
                {
                    var authService = sp.GetRequiredService<IAuthService>();
                    await authService.LoginAsync("non_existent_user_12345", "wrong_password");
                });
            });
        }
    }
}
