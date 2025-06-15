using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infrastructure.Entities.Crypto
{
    public class CryptoProviderDto: BaseEntity
    {
        public string Name { get; set; }

        public ICollection<CryptoAccountDto> CryptoAccounts { get; set; }
    }
}
