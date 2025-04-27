using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Integrations.Stock
{
    public interface IStockConnector
    {
        Task<Dictionary<string, decimal>> GetValuesByTickers(IEnumerable<string> tickers);
    }
}
