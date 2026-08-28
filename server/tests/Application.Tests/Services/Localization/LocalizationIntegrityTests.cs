using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using MoneyManager.Application.Constants;
using MoneyManager.Application.Services.Localization;
using Xunit;

namespace MoneyManager.Application.Tests.Services.Localization
{
    public class LocalizationIntegrityTests
    {
        private static readonly Assembly ApplicationAssembly = typeof(LocalizationService).Assembly;
        private static readonly string[] SupportedLocales = ["en", "ru"];

        private static readonly Regex ResourceNamePattern = new(
            @"\.Resources\.(?<lang>[a-zA-Z0-9_-]+)\.(?<cat>[a-zA-Z0-9_-]+)\.json$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static Dictionary<string, Dictionary<string, string>> LoadAllLocaleDictionaries()
        {
            var result = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

            foreach (var locale in SupportedLocales)
            {
                result[locale] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            foreach (var resourceName in ApplicationAssembly.GetManifestResourceNames())
            {
                var match = ResourceNamePattern.Match(resourceName);
                if (!match.Success) continue;

                var locale = match.Groups["lang"].Value.ToLowerInvariant();
                var category = match.Groups["cat"].Value.ToLowerInvariant();

                if (!result.ContainsKey(locale)) continue;

                using var stream = ApplicationAssembly.GetManifestResourceStream(resourceName)!;
                using var reader = new StreamReader(stream);
                var json = reader.ReadToEnd();

                using var doc = JsonDocument.Parse(json);
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    var fullKey = $"{category}.{prop.Name}".ToLowerInvariant();
                    result[locale][fullKey] = prop.Value.GetString() ?? string.Empty;
                }
            }

            return result;
        }

        private static HashSet<string> GetAllLocalizationConstants()
        {
            var constants = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            CollectConstants(typeof(LocalizationKeys), constants);
            return constants;
        }

        private static void CollectConstants(Type parentType, HashSet<string> constants)
        {
            var fields = parentType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string));

            foreach (var field in fields)
            {
                var val = (string)field.GetValue(null)!;
                constants.Add(val);
            }

            foreach (var nested in parentType.GetNestedTypes(BindingFlags.Public | BindingFlags.Static))
            {
                CollectConstants(nested, constants);
            }
        }

        [Fact]
        public void All_Localization_Files_Must_Exist_In_All_Locales()
        {
            var resourceNames = ApplicationAssembly.GetManifestResourceNames()
                .Where(r => r.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var enFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var ruFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var r in resourceNames)
            {
                if (r.Contains(".Resources.en.", StringComparison.OrdinalIgnoreCase))
                {
                    enFiles.Add(r.Replace(".Resources.en.", ".Resources.X.", StringComparison.OrdinalIgnoreCase));
                }
                else if (r.Contains(".Resources.ru.", StringComparison.OrdinalIgnoreCase))
                {
                    ruFiles.Add(r.Replace(".Resources.ru.", ".Resources.X.", StringComparison.OrdinalIgnoreCase));
                }
            }

            var missingInRu = enFiles.Except(ruFiles).ToList();
            var missingInEn = ruFiles.Except(enFiles).ToList();

            Assert.True(missingInRu.Count == 0, $"Files present in EN but missing in RU: {string.Join(", ", missingInRu)}");
            Assert.True(missingInEn.Count == 0, $"Files present in RU but missing in EN: {string.Join(", ", missingInEn)}");
        }

        [Fact]
        public void All_Keys_Must_Be_Symmetric_Between_Locales()
        {
            var dicts = LoadAllLocaleDictionaries();
            var enKeys = dicts["en"].Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
            var ruKeys = dicts["ru"].Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);

            var missingInRu = enKeys.Except(ruKeys).ToList();
            var missingInEn = ruKeys.Except(enKeys).ToList();

            Assert.True(missingInRu.Count == 0, $"Keys present in 'en' but missing in 'ru': {string.Join(", ", missingInRu)}");
            Assert.True(missingInEn.Count == 0, $"Keys present in 'ru' but missing in 'en': {string.Join(", ", missingInEn)}");
        }

        [Fact]
        public void All_Format_Placeholders_Must_Match_Across_Locales()
        {
            var dicts = LoadAllLocaleDictionaries();
            var enDict = dicts["en"];
            var ruDict = dicts["ru"];

            var placeholderRegex = new Regex(@"\{(\d+)\}", RegexOptions.Compiled);
            var errors = new List<string>();

            foreach (var key in enDict.Keys)
            {
                if (!ruDict.TryGetValue(key, out var ruValue)) continue;

                var enValue = enDict[key];

                var enPlaceholders = placeholderRegex.Matches(enValue).Select(m => m.Value).OrderBy(x => x).ToList();
                var ruPlaceholders = placeholderRegex.Matches(ruValue).Select(m => m.Value).OrderBy(x => x).ToList();

                if (!enPlaceholders.SequenceEqual(ruPlaceholders))
                {
                    errors.Add($"Placeholder mismatch for key '{key}': EN=[{string.Join(",", enPlaceholders)}] vs RU=[{string.Join(",", ruPlaceholders)}]");
                }
            }

            Assert.True(errors.Count == 0, $"Placeholder inconsistencies found:\n{string.Join("\n", errors)}");
        }

        [Fact]
        public void All_Localization_Constants_Must_Exist_In_Dictionaries()
        {
            var dicts = LoadAllLocaleDictionaries();
            var constants = GetAllLocalizationConstants();

            var missingInEn = new List<string>();
            var missingInRu = new List<string>();

            foreach (var constant in constants)
            {
                if (!dicts["en"].ContainsKey(constant))
                {
                    missingInEn.Add(constant);
                }

                if (!dicts["ru"].ContainsKey(constant))
                {
                    missingInRu.Add(constant);
                }
            }

            Assert.True(missingInEn.Count == 0, $"LocalizationKeys defined in C# but missing in EN dictionaries: {string.Join(", ", missingInEn)}");
            Assert.True(missingInRu.Count == 0, $"LocalizationKeys defined in C# but missing in RU dictionaries: {string.Join(", ", missingInRu)}");
        }

        [Fact]
        public void All_Dictionary_Keys_Must_Have_Corresponding_Constant()
        {
            var dicts = LoadAllLocaleDictionaries();
            var constants = GetAllLocalizationConstants();

            var unmappedKeys = dicts["en"].Keys
                .Where(k => !constants.Contains(k))
                .ToList();

            Assert.True(unmappedKeys.Count == 0, $"Keys present in JSON dictionaries but missing in LocalizationKeys constants: {string.Join(", ", unmappedKeys)}");
        }
    }
}
