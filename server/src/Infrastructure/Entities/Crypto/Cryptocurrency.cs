using Audex.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audex.Infrastructure.Entities.Crypto
{
    public class Cryptocurrency: BaseEntity
    {
        public string Name { get; set; }

        public string Symbol { get; set; }

        public decimal Price { get; set; } 

        public string IconKey { get; set; }

        public ICollection<CryptoAccountCryptocurrency> CryptoAccountCryptocurrencies { get; set; }
    }
}
