using System;

namespace Audex.WebApi.Models.Securities
{
    public class SecurityHistoryValueModel
    {
        public DateTime Date { get; set; }

        public decimal Value { get; set; }
    }
}
