using Audex.Infrastructure.Entities.Securities;
using Audex.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audex.Infrastructure.Entities.Crypto
{
    public class CryptoProvider: BaseEntity
    {
        public string Name { get; set; }

        public ICollection<CryptoAccount> CryptoAccounts { get; set; }
    }
}
