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
        Task<IEnumerable<MarketDataRow>> GetValuesByTickersAsync(IEnumerable<SecurityDto> tickers);

        Task<IEnumerable<SecurityHistoryValueDto>> GetTickerHistoryAsync(SecurityDto security, DateOnly from, DateOnly to);

        Task<IEnumerable<MarketDataRow>> GetExtendedValuesByTickersAsync(IEnumerable<SecurityDto> tickers);

        Task<IEnumerable<SecurityCandleDto>> GetCandlesAsync(SecurityDto security, DateOnly from, DateOnly to, int interval = 24);
    }
}
