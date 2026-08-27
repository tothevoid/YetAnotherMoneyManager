#nullable enable
using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MoneyManager.Application.Interfaces.Localization;
using MoneyManager.Application.Interfaces.User;

namespace MoneyManager.Application.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private const string DefaultLanguage = "en";
        private readonly ITranslationProvider _translationProvider;
        private readonly IUserProfileService _userProfileService;
        private readonly ILogger<LocalizationService> _logger;

        public LocalizationService(
            ITranslationProvider translationProvider,
            IUserProfileService userProfileService,
            ILogger<LocalizationService> logger)
        {
            _translationProvider = translationProvider;
            _userProfileService = userProfileService;
            _logger = logger;
        }

        public string Get(string key, string? languageCode = null, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            var template = _translationProvider.GetTranslation(languageCode, key);

            if (args != null && args.Length > 0)
            {
                try
                {
                    return string.Format(CultureInfo.InvariantCulture, template, args);
                }
                catch (FormatException ex)
                {
                    _logger.LogWarning(ex, "Failed to format localized template for key '{Key}' with language '{Language}'", key, languageCode);
                    return template;
                }
            }

            return template;
        }

        public async Task<string> GetUserLanguageAsync(Guid? userProfileId = null)
        {
            try
            {
                var user = await _userProfileService.GetAsync();
                return _translationProvider.NormalizeLanguageCode(user?.LanguageCode);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to resolve user language profile. Falling back to default language '{DefaultLanguage}'", DefaultLanguage);
                return DefaultLanguage;
            }
        }

        public async Task<string> GetForUserAsync(string key, Guid? userProfileId = null, params object[] args)
        {
            var userLang = await GetUserLanguageAsync(userProfileId);
            return Get(key, userLang, args);
        }
    }
}
