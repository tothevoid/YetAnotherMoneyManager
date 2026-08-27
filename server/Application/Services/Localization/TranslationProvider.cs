#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using MoneyManager.Application.Interfaces.Localization;

namespace MoneyManager.Application.Services.Localization
{
    public class TranslationProvider : ITranslationProvider
    {
        private const string DefaultLanguage = "en";
        private static readonly Regex ResourceNamePattern = new(
            @"\.Resources\.(?<lang>[a-zA-Z0-9_-]+)\.(?<cat>[a-zA-Z0-9_-]+)\.json$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _translations = new(StringComparer.OrdinalIgnoreCase);
        private readonly ILogger<TranslationProvider>? _logger;

        public TranslationProvider(ILogger<TranslationProvider>? logger = null)
        {
            _logger = logger;
            LoadEmbeddedTranslations();
        }

        public string GetTranslation(string? languageCode, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            var normalizedLang = NormalizeLanguageCode(languageCode);

            // 1. Try specified language
            if (_translations.TryGetValue(normalizedLang, out var langDict) && langDict.TryGetValue(key, out var translation))
            {
                return translation;
            }

            // 2. Fallback to English
            if (!string.Equals(normalizedLang, DefaultLanguage, StringComparison.OrdinalIgnoreCase))
            {
                if (_translations.TryGetValue(DefaultLanguage, out var defaultDict) && defaultDict.TryGetValue(key, out var defaultTranslation))
                {
                    return defaultTranslation;
                }
            }

            // 3. Fallback to key itself
            return key;
        }

        public string NormalizeLanguageCode(string? languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                return DefaultLanguage;
            }

            var clean = languageCode.Trim().ToLowerInvariant();

            if (clean.StartsWith("ru", StringComparison.OrdinalIgnoreCase))
            {
                return "ru";
            }

            if (clean.StartsWith("en", StringComparison.OrdinalIgnoreCase))
            {
                return "en";
            }

            return DefaultLanguage;
        }

        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> GetAllTranslations()
        {
            var result = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var translation in _translations)
            {
                result[translation.Key] = translation.Value;
            }
            return result;
        }

        private void LoadEmbeddedTranslations()
        {
            var assembly = typeof(TranslationProvider).Assembly;

            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                var match = ResourceNamePattern.Match(resourceName);
                if (!match.Success)
                {
                    continue;
                }

                var language = match.Groups["lang"].Value.ToLowerInvariant();
                var category = match.Groups["cat"].Value.ToLowerInvariant();

                try
                {
                    using var stream = assembly.GetManifestResourceStream(resourceName);
                    if (stream == null) continue;

                    using var reader = new StreamReader(stream);
                    var jsonContent = reader.ReadToEnd();

                    using var doc = JsonDocument.Parse(jsonContent);
                    var langDict = _translations.GetOrAdd(language, _ => new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase));

                    foreach (var prop in doc.RootElement.EnumerateObject())
                    {
                        var value = prop.Value.GetString() ?? string.Empty;

                        var fullKey = $"{category}.{prop.Name}";
                        langDict[fullKey] = value;
                        langDict[prop.Name] = value;
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Failed to load embedded localization resource '{ResourceName}'", resourceName);
                }
            }
        }
    }
}
