using ProjectB.IO;
using ProjectB.Models;

namespace ProjectB.Repositories;

public abstract class AbstractRepository<TEntity, TId>: IRepository<TEntity, TId>
    where TEntity: IEntity<TId>
    where TId: notnull
{

    protected readonly Dictionary<TId, TEntity> Repository = new();

    public void Save(TEntity entity) => Repository.TryAdd(entity.GetId()!, entity);

    public TEntity FindById(TId id) => Repository[id];

    public IEnumerable<TEntity> FindAll() => Repository.Values;
    public void Remove(TId id) => Repository.Remove(id);

    public void RemoveAll() => Repository.Clear();

    protected AbstractRepository()
    {
        Refresh();
    }
    
    public void Refresh()
    {
        if (!File.Exists(GetFileLocation()))
        {
            File.Create(GetFileLocation()).Close();
        }
        
        JsonFileReader<TEntity> reader = new();
        ICollection<TEntity>? entities = reader.ReadAllObjects(GetFileLocation());
        if (entities != null)
        {
            Repository.Clear();
            foreach (TEntity entity in entities)
            {
                Repository.Add(entity.GetId()!, entity);
            }
        }
    }

    public void Persist()
    {
        JsonFileWriter<TEntity> writer = new();
        writer.WriteObjects(GetFileLocation(), Repository.Values);
    }

    private string GetFileLocation()
    {
        return $".//../../../Database/{GetType().Name}.json";
    }
    
    public int Count() => Repository.Count;
    public bool Exists(TEntity entity) => Exists(entity.GetId());

    public bool Exists(TId? id)
    {
        return id != null && Repository.ContainsKey(id);
    }
}