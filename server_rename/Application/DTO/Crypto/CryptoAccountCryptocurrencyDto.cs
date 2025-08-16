using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infrastructure.Entities.Crypto
{
    public class CryptoAccountCryptocurrencyDto: BaseEntity
    {
        public Guid CrpytocurrencyId { get; set; }

        public CryptocurrencyDto Cryptocurrency { get; set; }

        public Guid CryptoAccountId { get; set; }

        public CryptoAccountDto CryptoAccount { get; set; }
        
        public decimal Quantity { get; set; }
    }
}
