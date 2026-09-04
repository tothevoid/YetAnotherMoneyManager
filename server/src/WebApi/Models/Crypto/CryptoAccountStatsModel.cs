using System.Collections.Generic;
using Audex.WebApi.Models.Dashboard;

namespace Audex.WebApi.Models.Crypto
{
    public class CryptoAccountStatsModel
    {
        public decimal TotalUsd { get; set; }

        public decimal TotalConverted { get; set; }

        public string MainCurrency { get; set; }

        public IEnumerable<DistributionModel> CryptoDistribution { get; set; }

        public IEnumerable<DistributionModel> AccountsDistribution { get; set; }
    }
}
