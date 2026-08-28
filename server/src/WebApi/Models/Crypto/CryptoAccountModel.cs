using Audex.Shared.Entities;
using System;
using System.Collections.Generic;

namespace Audex.WebApi.Models.Crypto
{
    public class CryptoAccountModel: BaseEntity
    {
        public string Name { get; set; }

        public Guid CryptoProviderId { get; set; }

        public CryptoProviderModel CryptoProvider { get; set; }
    }
}
