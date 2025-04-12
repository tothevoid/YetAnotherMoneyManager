using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Interfaces.Utilitary
{
    public interface IDataGenerator<out TEntity>
        where TEntity: BaseEntity
    {
        public TEntity[] Generate();
    }
}
