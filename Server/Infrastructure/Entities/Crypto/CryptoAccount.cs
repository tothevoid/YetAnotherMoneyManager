using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;

namespace MoneyManager.Infrastructure.Entities.Crypto
{
    public class CryptoAccount: BaseEntity
    {
        public string Name { get; set; }

        public Guid CryptoProviderId { get; set; }

        public CryptoProvider CryptoProvider { get; set; }

        public ICollection<CryptoAccountCryptocurrency> CryptoAccountCryptocurrencies { get; set; }
    }
}
