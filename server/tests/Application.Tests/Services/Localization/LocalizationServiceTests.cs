using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Audex.Application.DTO;
using Audex.Application.DTO.User;
using Audex.Application.Interfaces.Localization;
using Audex.Application.Interfaces.User;
using Audex.Application.Services.Localization;
using Audex.Infrastructure.Constants;
using Audex.Tests.Shared.Fixtures;
using Xunit;

namespace Audex.Application.Tests.Services.Localization
{
    public class LocalizationServiceTests : TestBase
    {
        public LocalizationServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task TestLocalization_English_ReturnsCorrectText()
        {
            await ExecuteScopeAsync(sp =>
            {
                var localizer = sp.GetRequiredService<ILocalizationService>();

                var title = localizer.Get("session_cleanup_title", "en");
                Assert.Equal("Session Tokens Clean Up", title);

                var dottedTitle = localizer.Get("notifications.session_cleanup_title", "en");
                Assert.Equal("Session Tokens Clean Up", dottedTitle);

                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task TestLocalization_Russian_ReturnsCorrectText()
        {
            await ExecuteScopeAsync(sp =>
            {
                var localizer = sp.GetRequiredService<ILocalizationService>();

                var title = localizer.Get("session_cleanup_title", "ru");
                Assert.Equal("Очистка сессионных токенов", title);

                var dottedTitle = localizer.Get("notifications.session_cleanup_title", "ru-RU");
                Assert.Equal("Очистка сессионных токенов", dottedTitle);

                return Task.CompletedTask;
            });
        }

        [Theory]
        [InlineData("en-US", "en")]
        [InlineData("EN", "en")]
        [InlineData("en", "en")]
        [InlineData("ru-RU", "ru")]
        [InlineData("RU", "ru")]
        [InlineData("ru", "ru")]
        [InlineData(null, "en")]
        [InlineData("", "en")]
        [InlineData("   ", "en")]
        public void TestLocalization_LanguageCodeNormalization(string? inputLang, string expectedNormalized)
        {
            var provider = new TranslationProvider();
            var result = provider.NormalizeLanguageCode(inputLang);
            Assert.Equal(expectedNormalized, result);
        }

        [Fact]
        public async Task TestLocalization_ArgumentsFormatting()
        {
            await ExecuteScopeAsync(sp =>
            {
                var localizer = sp.GetRequiredService<ILocalizationService>();

                var enMessage = localizer.Get("session_cleanup_message", "en", 42);
                Assert.Equal("Removed 42 expired or revoked tokens.", enMessage);

                var ruMessage = localizer.Get("session_cleanup_message", "ru", 42);
                Assert.Equal("Удалено устаревших или отозванных токенов: 42.", ruMessage);

                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task TestLocalization_FallbackToEnglish_WhenKeyMissingInRussian()
        {
            await ExecuteScopeAsync(sp =>
            {
                var localizer = sp.GetRequiredService<ILocalizationService>();

                // A key that exists in English should fall back to English if requested in an unknown language
                var text = localizer.Get("session_cleanup_title", "de-DE");
                Assert.Equal("Session Tokens Clean Up", text);

                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task TestLocalization_FallbackToKey_WhenKeyMissingInAllLanguages()
        {
            await ExecuteScopeAsync(sp =>
            {
                var localizer = sp.GetRequiredService<ILocalizationService>();

                var missingKey = "non_existent_key_12345";
                var result = localizer.Get(missingKey, "ru");
                Assert.Equal(missingKey, result);

                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task TestLocalization_GetForUserAsync_ResolvesUserLanguage()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var localizer = sp.GetRequiredService<ILocalizationService>();
                var userService = sp.GetRequiredService<IUserProfileService>();

                // Set user language to Russian
                await userService.UpdateAsync(new UserProfileDto
                {
                    Id = UserProfileConstants.UserProfileId,
                    LanguageCode = "ru-RU"
                });

                var ruTitle = await localizer.GetForUserAsync("notifications.session_cleanup_title", UserProfileConstants.UserProfileId);
                Assert.Equal("Очистка сессионных токенов", ruTitle);

                // Set user language to English
                await userService.UpdateAsync(new UserProfileDto
                {
                    Id = UserProfileConstants.UserProfileId,
                    LanguageCode = "en-US"
                });

                var enTitle = await localizer.GetForUserAsync("notifications.session_cleanup_title", UserProfileConstants.UserProfileId);
                Assert.Equal("Session Tokens Clean Up", enTitle);
            });
        }

        [Fact]
        public async Task TestLocalization_GetUserLanguageAsync_ResolvesNormalizedLanguage()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var localizer = sp.GetRequiredService<ILocalizationService>();
                var userService = sp.GetRequiredService<IUserProfileService>();

                // Set user language to Russian
                await userService.UpdateAsync(new UserProfileDto
                {
                    Id = UserProfileConstants.UserProfileId,
                    LanguageCode = "ru-RU"
                });

                var ruLang = await localizer.GetUserLanguageAsync();
                Assert.Equal("ru", ruLang);

                // Set user language to English
                await userService.UpdateAsync(new UserProfileDto
                {
                    Id = UserProfileConstants.UserProfileId,
                    LanguageCode = "en-US"
                });

                var enLang = await localizer.GetUserLanguageAsync();
                Assert.Equal("en", enLang);
            });
        }
    }
}
