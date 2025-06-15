using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.WebApi.Models.Crypto
{
    public class CryptocurrencyModel: BaseEntity
    {
        public string Name { get; set; }

        public string Symbol { get; set; }

        public decimal Price { get; set; } 

        public string IconKey { get; set; }

        public ICollection<CryptoAccountCryptocurrencyModel> CryptoAccountCryptocurrencies { get; set; }
    }
}
