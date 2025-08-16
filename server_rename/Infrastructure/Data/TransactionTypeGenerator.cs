using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Infrastructure.Interfaces.Utilitary;

namespace MoneyManager.Infrastructure.Data
{
    public class TransactionTypeGenerator : IDataGenerator<TransactionType>
    {
        public TransactionType[] Generate()
        {
            return new TransactionType[]
            {
                new() { Id = TransactionTypeConstants.System, Name = "System"},
            };
        }
    }
}