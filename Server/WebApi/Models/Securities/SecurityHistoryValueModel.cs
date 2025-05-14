using System;

namespace MoneyManager.WebApi.Models.Securities
{
    public class SecurityHistoryValueModel
    {
        public DateOnly Date { get; set; }

        public decimal Value { get; set; }
    }
}
