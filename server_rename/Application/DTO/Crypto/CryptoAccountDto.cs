using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;

namespace MoneyManager.Infrastructure.Entities.Crypto
{
    public class CryptoAccountDto: BaseEntity
    {
        public string Name { get; set; }

        public Guid CryptoProviderId { get; set; }

        public CryptoProviderDto CryptoProvider { get; set; }

        public ICollection<CryptoAccountCryptocurrencyDto> CryptoAccountCryptocurrencies { get; set; }
    }
}
