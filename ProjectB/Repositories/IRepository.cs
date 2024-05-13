using ProjectB.Models;

namespace ProjectB.Repositories;

/**
 * Base for every repository used in this program.
 */
public interface IRepository<TEntity, TId>
    where TEntity : IEntity<TId>
    where TId : notnull
{

    
    void Save(TEntity entity);
    
    TEntity? FindById(TId id);
    
    IEnumerable<TEntity> FindAll();

    void Remove(TId id);

    void Remove(TEntity entity) => Remove(entity.GetId());

    void RemoveAll();

    void Refresh();

    void Persist();

    int Count();
    
}