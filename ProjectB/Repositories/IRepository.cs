using ProjectB.Models;

namespace ProjectB.Repositories;

/**
 * Base for every repository used in this program.
 */
public interface IRepository<TEntity>
    where TEntity : IEntity
{

    
    void Save(TEntity entity);
    
    TEntity? FindById(long id);
    
    IEnumerable<TEntity> FindAll();

    void Remove(long id);

    void Remove(TEntity entity) => Remove(entity.GetId());

    void RemoveAll();

    void Refresh();

    void Persist();

    int Count();
    
}