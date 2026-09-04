using System.Collections.Generic;
using Audex.Application.DTO.Dashboard;

namespace Audex.Application.DTO.Crypto
{
    public class CryptoAccountStatsDto
    {
        public decimal TotalUsd { get; set; }

        public decimal TotalConverted { get; set; }

        public string MainCurrency { get; set; }

        public IEnumerable<DistributionDto> CryptoDistribution { get; set; }

        public IEnumerable<DistributionDto> AccountsDistribution { get; set; }
    }
}
