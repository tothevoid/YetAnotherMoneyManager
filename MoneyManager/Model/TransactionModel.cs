using MoneyManager.Common;
using MoneyManager.DAL;
using System;

namespace MoneyManager.WEB.Model
{
    public class TransactionModel: BaseEntity
    {
        public string Name {get;set;}

        public DateTime Date {get;set;}

        public double MoneyQuantity {get;set;}

        public FundModel FundSource {get;set;}
    }
}