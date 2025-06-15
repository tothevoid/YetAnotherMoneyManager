using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.WebApi.Models.Crypto
{
    public class CryptoProviderModel: BaseEntity
    {
        public string Name { get; set; }

        public ICollection<CryptoAccountModel> CryptoAccounts { get; set; }
    }
}
