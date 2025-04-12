using MoneyManager.Common;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace MoneyManager.DAL.Interfaces
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