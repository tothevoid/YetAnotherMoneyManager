using Audex.Infrastructure.Constants;
using Audex.Infrastructure.Entities.Securities;
using Audex.Infrastructure.Entities.Transactions;
using Audex.Infrastructure.Interfaces.Utilitary;

namespace Audex.Infrastructure.Data
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