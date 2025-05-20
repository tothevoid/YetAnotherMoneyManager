using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Integrations.Currency
{
    public interface ICurrencyGrabber
    {
        public Task<Dictionary<string, decimal>> GetCurrency(string mainCurrency, HashSet<string> requestedCurrencies);
    }
}
