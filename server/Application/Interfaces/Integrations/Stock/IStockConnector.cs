using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Integrations.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Application.Integrations.Stock.Moex.Model;

namespace MoneyManager.Application.Interfaces.Integrations.Stock
{
    public interface IStockConnector
    {
        Task<IEnumerable<MarketDataRow>> GetValuesByTickers(IEnumerable<SecurityDTO> tickers);

        Task<IEnumerable<SecurityHistoryValueDto>> GetTickerHistory(SecurityDTO security, DateOnly from, DateOnly to);

        Task<IEnumerable<MarketDataRow>> GetExtendedValuesByTickers(IEnumerable<SecurityDTO> tickers);
    }
}
