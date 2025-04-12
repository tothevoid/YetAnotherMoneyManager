using MoneyManager.Infrastructure.Interfaces.Repositories;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Interfaces.Database
{
    public interface IUnitOfWork
    {
        IRepository<T> CreateRepository<T>() 
            where T: BaseEntity;

        ITransactionRepository CreateTransactionRepository();

        IDepositRepository CreateDepositRepository();

        IAccountRepository CreateAccountRepository();

        void Commit();
    }
}