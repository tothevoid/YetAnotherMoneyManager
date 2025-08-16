using MoneyManager.Shared.Entities;
using System;

namespace MoneyManager.WebApi.Models.Crypto
{
    public class CryptoAccountCryptocurrencyModel: BaseEntity
    {
        public Guid CrpytocurrencyId { get; set; }

        public CryptocurrencyModel Cryptocurrency { get; set; }

        public Guid CryptoAccountId { get; set; }

        public CryptoAccountModel CryptoAccount { get; set; }
        
        public decimal Quantity { get; set; }
    }
}
