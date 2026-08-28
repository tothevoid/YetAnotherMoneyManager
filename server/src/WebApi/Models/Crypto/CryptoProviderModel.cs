using Audex.Infrastructure.Entities.Securities;
using Audex.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audex.WebApi.Models.Crypto
{
    public class CryptoProviderModel: BaseEntity
    {
        public string Name { get; set; }
    }
}
