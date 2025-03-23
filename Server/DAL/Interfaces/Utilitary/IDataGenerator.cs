using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Common;
using MoneyManager.DAL.Entities;

namespace DAL.Interfaces.Utilitary
{
    public interface IDataGenerator<out TEntity>
        where TEntity: BaseEntity
    {
        public TEntity[] Generate();
    }
}
