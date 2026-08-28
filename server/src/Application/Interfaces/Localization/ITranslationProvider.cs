#nullable enable
using System.Collections.Generic;

namespace Audex.Application.Interfaces.Localization
{
    public interface ITranslationProvider
    {
        string GetTranslation(string? languageCode, string key);
        string NormalizeLanguageCode(string? languageCode);
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> GetAllTranslations();
    }
}
