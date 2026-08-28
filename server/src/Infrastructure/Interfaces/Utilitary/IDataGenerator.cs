using Audex.Shared.Entities;

namespace Audex.Infrastructure.Interfaces.Utilitary
{
    public interface IDataGenerator<out TEntity>
        where TEntity: BaseEntity
    {
        public TEntity[] Generate();
    }
}
