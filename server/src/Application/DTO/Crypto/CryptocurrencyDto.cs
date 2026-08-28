using Audex.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audex.Application.DTO.Crypto
{
    public class CryptocurrencyDto: BaseEntity
    {
        public string Name { get; set; }

        public string Symbol { get; set; }

        public decimal Price { get; set; } 

        public string IconKey { get; set; }
    }
}
