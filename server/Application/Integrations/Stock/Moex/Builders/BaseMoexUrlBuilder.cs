using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyManager.Application.Integrations.Stock.Moex.Builders
{
    public class BaseMoexUrlBuilder
    {
        protected string Url = "https://iss.moex.com/iss";

        protected readonly List<string> AdditionalParameters = new();

        protected readonly List<string> OutputFilters = new();

        public BaseMoexUrlBuilder RemoveMeta()
        {
            AdditionalParameters.Add("iss.meta=off");
            return this;
        }

        public BaseMoexUrlBuilder AddRange(DateOnly from, DateOnly to)
        {
            var pattern = "yyyy-MM-dd";
            AdditionalParameters.Add($"from={from.ToString(pattern)}&till={to.ToString(pattern)}");
            return this;
        }

        public virtual string Build()
        {
            if (OutputFilters.Any())
            {
                AdditionalParameters.Add($"iss.only={string.Join(",", OutputFilters)}");
            }

            if (AdditionalParameters.Any())
            {
                var combinedParameters = string.Join("&", AdditionalParameters);
                return $"{Url}?{combinedParameters}";
            }

            return Url;
        }
    }
}
