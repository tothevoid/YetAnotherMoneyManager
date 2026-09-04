using System.Collections.Generic;
using Audex.Application.DTO.Dashboard;

namespace Audex.Application.DTO.Crypto
{
    public class CryptoAccountStatsDto
    {
        public IEnumerable<DistributionDto> CryptoDistribution { get; set; }

        public IEnumerable<DistributionDto> AccountsDistribution { get; set; }
    }
}
