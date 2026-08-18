using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Auth;
using MoneyManager.Application.Interfaces.Auth;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Application.Services.Auth;
using MoneyManager.Application.Tests.Fixtures;
using System;
using System.Security;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.Application.Tests.Services.Auth
{
    [Trait("Category", "Auth")]
    public class AuthServiceTests : TestBase
    {
        public AuthServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
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
            var loginResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "");
            });

            Assert.NotNull(loginResult);
            Assert.False(string.IsNullOrWhiteSpace(loginResult.AccessToken));
            Assert.False(string.IsNullOrWhiteSpace(loginResult.RefreshToken));

            // Test ChangePassword
            var newPassword = "newPassword_" + Guid.NewGuid().ToString("N");

            var changeResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.ChangePasswordAsync(user.UserName, user.Password ?? "", newPassword);
            });

            Assert.True(changeResult);

            // Test Login with new password
            var newLoginResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, newPassword);
            });

            Assert.NotNull(newLoginResult);
            Assert.False(string.IsNullOrWhiteSpace(newLoginResult.AccessToken));
            Assert.False(string.IsNullOrWhiteSpace(newLoginResult.RefreshToken));
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

        [Fact]
        public async Task TestRefreshToken_SuccessfulRotation()
        {
            var user = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            var loginResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "");
            });

            // Perform refresh
            var refreshResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.RefreshTokenAsync(loginResult.RefreshToken);
            });

            Assert.NotNull(refreshResult);
            Assert.False(string.IsNullOrWhiteSpace(refreshResult.AccessToken));
            Assert.False(string.IsNullOrWhiteSpace(refreshResult.RefreshToken));
            Assert.NotEqual(loginResult.RefreshToken, refreshResult.RefreshToken);

            // Subsequent refresh using the newly rotated token should succeed
            var secondRefreshResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.RefreshTokenAsync(refreshResult.RefreshToken);
            });

            Assert.NotNull(secondRefreshResult);
            Assert.False(string.IsNullOrWhiteSpace(secondRefreshResult.AccessToken));
            Assert.False(string.IsNullOrWhiteSpace(secondRefreshResult.RefreshToken));
        }

        [Fact]
        public async Task TestRefreshToken_WithinGracePeriod_Succeeds()
        {
            var user = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            var loginResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "");
            });

            // Legitimate refresh rotates the token
            var refreshResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.RefreshTokenAsync(loginResult.RefreshToken);
            });

            Assert.NotNull(refreshResult);

            // Concurrent request within 30s grace window sends the old login token again
            var concurrentResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.RefreshTokenAsync(loginResult.RefreshToken);
            });

            Assert.NotNull(concurrentResult);
            Assert.False(string.IsNullOrWhiteSpace(concurrentResult.AccessToken));
        }

        [Fact]
        public async Task TestRefreshToken_ReuseDetection_RevokesAllSessions()
        {
            var user = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            var loginResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "");
            });

            // Legitimate refresh rotates the token (old token marked as used)
            var refreshResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.RefreshTokenAsync(loginResult.RefreshToken);
            });

            Assert.NotNull(refreshResult);

            // Simulate that the replacement token was created outside the 30-second grace window (e.g. 2 minutes ago)
            await ExecuteScopeAsync(async sp =>
            {
                var uow = sp.GetRequiredService<MoneyManager.Infrastructure.Interfaces.Database.IUnitOfWork>();
                var tokenRepo = uow.CreateRepository<MoneyManager.Infrastructure.Entities.User.UserRefreshToken>();
                var tokens = await tokenRepo.GetAllAsync(t => t.UserProfileId == user.Id);
                foreach (var t in tokens)
                {
                    t.CreatedAt = DateTime.UtcNow.AddMinutes(-2);
                    tokenRepo.Update(t);
                }
                await uow.CommitAsync();
            });

            // Attacker tries to reuse the old loginResult.RefreshToken outside grace window
            await Assert.ThrowsAsync<SecurityException>(async () =>
            {
                await ExecuteScopeAsync(async sp =>
                {
                    var authService = sp.GetRequiredService<IAuthService>();
                    await authService.RefreshTokenAsync(loginResult.RefreshToken);
                });
            });

            // Because reuse was detected, all tokens (including refreshResult.RefreshToken) are now revoked
            await Assert.ThrowsAsync<SecurityException>(async () =>
            {
                await ExecuteScopeAsync(async sp =>
                {
                    var authService = sp.GetRequiredService<IAuthService>();
                    await authService.RefreshTokenAsync(refreshResult.RefreshToken);
                });
            });
        }

        [Fact]
        public async Task TestRevokeToken_Success()
        {
            var user = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            var loginResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "");
            });

            var revoked = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.RevokeTokenAsync(loginResult.RefreshToken);
            });

            Assert.True(revoked);

            // Trying to refresh revoked token should fail
            await Assert.ThrowsAsync<SecurityException>(async () =>
            {
                await ExecuteScopeAsync(async sp =>
                {
                    var authService = sp.GetRequiredService<IAuthService>();
                    await authService.RefreshTokenAsync(loginResult.RefreshToken);
                });
            });
        }

        [Fact]
        public async Task TestRevokeAllUserTokens_Success()
        {
            var user = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            var session1 = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "");
            });

            var session2 = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "");
            });

            var revoked = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.RevokeAllUserTokensAsync(user.Id);
            });

            Assert.True(revoked);

            // Both sessions should be revoked
            await Assert.ThrowsAsync<SecurityException>(async () =>
            {
                await ExecuteScopeAsync(async sp =>
                {
                    var authService = sp.GetRequiredService<IAuthService>();
                    await authService.RefreshTokenAsync(session1.RefreshToken);
                });
            });

            await Assert.ThrowsAsync<SecurityException>(async () =>
            {
                await ExecuteScopeAsync(async sp =>
                {
                    var authService = sp.GetRequiredService<IAuthService>();
                    await authService.RefreshTokenAsync(session2.RefreshToken);
                });
            });
        }

        [Fact]
        public async Task TestGetRefreshTokensAndPagination_ActiveAndInactive()
        {
            var user = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            // Login 2 sessions
            var session1 = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "", "192.168.1.1", "Chrome on Windows");
            });

            var session2 = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "", "10.0.0.1", "Safari on iPhone");
            });

            // Get active tokens
            var activeTokens = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.GetRefreshTokensAsync(user.Id, isActive: true, 1, 10, session1.RefreshToken);
            });

            Assert.NotNull(activeTokens);
            Assert.Contains(activeTokens, s => s.IsCurrent && s.CreatedByIp == "192.168.1.1");
            Assert.Contains(activeTokens, s => !s.IsCurrent && s.CreatedByIp == "10.0.0.1");

            // Pagination config
            var paginationConfig = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.GetRefreshTokensPaginationAsync(user.Id, isActive: true);
            });

            Assert.NotNull(paginationConfig);
            Assert.True(paginationConfig.RecordsQuantity >= 2);
        }

        [Fact]
        public async Task TestRevokeSingleToken_Success()
        {
            var user = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            var session = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "", "192.168.1.50", "Firefox on Linux");
            });

            var activeTokens = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.GetRefreshTokensAsync(user.Id, isActive: true);
            });

            var targetToken = Assert.Single(activeTokens, s => s.CreatedByIp == "192.168.1.50");

            var revoked = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.RevokeTokenAsync(targetToken.Id, user.Id);
            });

            Assert.True(revoked);

            // Verify session is no longer active and fails refresh
            await Assert.ThrowsAsync<SecurityException>(async () =>
            {
                await ExecuteScopeAsync(async sp =>
                {
                    var authService = sp.GetRequiredService<IAuthService>();
                    await authService.RefreshTokenAsync(session.RefreshToken);
                });
            });
        }

        [Fact]
        public async Task TestRevokeOtherTokens_Success()
        {
            var user = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            var currentSession = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "", "192.168.1.100", "Current Browser");
            });

            var otherSession = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "", "192.168.1.200", "Other Browser");
            });

            var revokedOthers = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.RevokeOtherTokensAsync(user.Id, currentSession.RefreshToken);
            });

            Assert.True(revokedOthers);

            // Current session should still be refreshable
            var refreshedCurrent = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.RefreshTokenAsync(currentSession.RefreshToken);
            });

            Assert.NotNull(refreshedCurrent);
            Assert.False(string.IsNullOrEmpty(refreshedCurrent.AccessToken));

            // Other session must be revoked and fail refresh
            await Assert.ThrowsAsync<SecurityException>(async () =>
            {
                await ExecuteScopeAsync(async sp =>
                {
                    var authService = sp.GetRequiredService<IAuthService>();
                    await authService.RefreshTokenAsync(otherSession.RefreshToken);
                });
            });
        }

        [Fact]
        public async Task TestCleanUpExpiredRefreshTokens()
        {
            var user = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IUserProfileService>();
                return await service.GetAsync();
            });

            Assert.NotNull(user);

            // Create a token
            var loginResult = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.LoginAsync(user.UserName, user.Password ?? "");
            });

            Assert.NotNull(loginResult);

            // Clean up tokens older than -1 day (should delete the token just created)
            var deletedCount = await ExecuteScopeAsync(async sp =>
            {
                var authService = sp.GetRequiredService<IAuthService>();
                return await authService.CleanUpExpiredRefreshTokensAsync(olderThanDays: -1);
            });

            Assert.True(deletedCount >= 1);
        }
    }
}
