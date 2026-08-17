using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Integrations.Currency
{
    public interface ICurrencyGrabber
    {
        public Task<Dictionary<string, decimal>> GetRatesAsync(string mainCurrency, HashSet<string> requestedCurrencies);
    }
}
