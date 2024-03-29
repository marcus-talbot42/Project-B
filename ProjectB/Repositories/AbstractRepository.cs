using ProjectB.Models;

namespace ProjectB.Repositories;

public abstract class AbstractRepository<TEntity, TId>: IRepository<TEntity, TId>
    where TEntity: Entity<TId>
    where TId: class
{

    protected readonly Dictionary<TId, TEntity> Repository = new();

    public void Save(TEntity entity) => Repository.TryAdd(entity.GetId(), entity);

    public TEntity? FindById(TId id) => Repository[id];

    public IEnumerable<TEntity> FindAll() => Repository.Values;
    public void Remove(TId id) => Repository.Remove(id);

    public void RemoveAll() => Repository.Clear();
    
    public abstract void Refresh();
    
    public abstract void Persist();
    public int Count() => Repository.Count;
}