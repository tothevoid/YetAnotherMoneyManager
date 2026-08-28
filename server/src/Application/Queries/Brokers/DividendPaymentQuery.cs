using Microsoft.EntityFrameworkCore;
using MoneyManager.Infrastructure.Entities.Brokers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Queries.Brokers
{
    public static class DividendPaymentQuery
    {
        public static IQueryable<DividendPayment> GetFullHierarchyColumns(IQueryable<DividendPayment> dividendPaymentQuery)
        {
            return dividendPaymentQuery
                .Include(dividendPayment => dividendPayment.Dividend.Security.Currency)
                .Include(dividendPayment => dividendPayment.Dividend.Security.Type)
                .Include(dividendPayment => dividendPayment.BrokerAccount.Type)
                .Include(dividendPayment => dividendPayment.BrokerAccount.Currency)
                .Include(dividendPayment => dividendPayment.BrokerAccount.Broker)
                .Include(dividendPayment => dividendPayment.BrokerAccount.Bank);
        }
    }
}
