using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infrastructure.Entities.Crypto
{
    public class CryptoAccountCryptocurrency: BaseEntity
    {
        public Guid CrpytocurrencyId { get; set; }

        public Cryptocurrency Cryptocurrency { get; set; }

        public Guid CryptoAccountId { get; set; }

        public CryptoAccount CryptoAccount { get; set; }
        
        public decimal Quantity { get; set; }
    }
}
