using System.Collections.Generic;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    public interface IRepository<TEntity, in TKey> where TEntity : IEntity
    {
        //IEnumerable<TEntity> List { get; }
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        TEntity FindById(TKey id);

        void SaveLocal(IEnumerable<TEntity> entities);
        SalesforceContext GetContext { get; }
    }
}