using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;

namespace MoneyManager.WebApi.Models.Crypto
{
    public class CryptoAccountModel: BaseEntity
    {
        public string Name { get; set; }

        public Guid CryptoProviderId { get; set; }

        public CryptoProviderModel CryptoProvider { get; set; }
    }
}
