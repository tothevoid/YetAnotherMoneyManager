using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;

namespace MoneyManager.Application.DTO.Crypto
{
    public class CryptoAccountDto: BaseEntity
    {
        public string Name { get; set; }

        public Guid CryptoProviderId { get; set; }

        public CryptoProviderDto CryptoProvider { get; set; }
    }
}
