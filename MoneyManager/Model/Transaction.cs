using System;

namespace MoneyManager.Model
{
    public class Transaction
    {
        public Guid Id {get;set;}

        public string Name {get;set;}

        public DateTime Date {get;set;}

        public int MoneyQuantity {get;set;}

        public string Description {get;set;}

        public int Type {get;set;}
    }
}