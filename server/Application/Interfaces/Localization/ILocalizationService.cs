#nullable enable
using System;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Localization
{
    public interface ILocalizationService
    {
        string Get(string key, string? languageCode = null, params object[] args);
        Task<string> GetForUserAsync(string key, Guid? userProfileId = null, params object[] args);
    }
}
