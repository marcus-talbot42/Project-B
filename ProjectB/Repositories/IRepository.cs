using ProjectB.Models;

namespace ProjectB.Repositories;

/**
 * Base for every repository used in this program.
 */
public interface IRepository<TEntity, TId>
    where TEntity : IEntity<TId>
    where TId : notnull
{

    /**
     * Saves the given entity to memory, and persist the data to the filesystem as a JSON-file.
     */
    void Save(TEntity entity);
    
    /**
     * Finds the entity with the corresponding ID. Returns null if none exist.
     */
    TEntity? FindById(TId id);
    
    /**
     * Returns every entity registered to this repository.
     */
    IEnumerable<TEntity> FindAll();

    void Remove(TId id);

    void Remove(TEntity entity) => Remove(entity.GetId());

    void RemoveAll();

    void Refresh();

    void Persist();

    int Count();
    
    string GetFileLocation();

}