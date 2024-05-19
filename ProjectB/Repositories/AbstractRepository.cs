using Microsoft.EntityFrameworkCore;
using ProjectB.Database;
using ProjectB.IO;
using ProjectB.Models;

namespace ProjectB.Repositories;

public abstract class AbstractRepository<TEntity, TId>(DatabaseContext databaseContext): IRepository<TEntity, TId>
where TEntity : class, IEntity<TId>
where TId : notnull
{

    protected DbSet<TEntity> DbSet { get; } = databaseContext.GetRelevantDbSet<TEntity, TId>()!;

    public void Save(TEntity entity)
    {
        DbSet.Add(entity: entity);
        databaseContext.SaveChanges();
        Persist();
    }

    public TEntity? FindById(TId id) => DbSet.Find(id);

    public IEnumerable<TEntity> FindAll() => DbSet.ToList();
    public void Remove(TId id) => DbSet.Remove(DbSet.Find(id)!);

    public void RemoveAll() => DbSet.RemoveRange(DbSet.ToArray());
    
    public void Refresh()
    {
        JsonFileReader<TEntity> reader = new();
        ICollection<TEntity>? entities = reader.ReadAllObjects(GetFileLocation());
        if (entities != null)
        {
            RemoveAll();
            DbSet.AddRange(entities);
        }
    }

    public void Persist()
    {
        File.CreateText(this.GetFileLocation()).Close();
        JsonFileWriter<TEntity> writer = new();
        writer.WriteObjects(GetFileLocation(), DbSet.ToList());
    }

    public string GetFileLocation() => $".//../../../Database/{typeof(TEntity).Name}.json";
    
    public int Count() => DbSet.Count();

    public bool Exists(TEntity entity) {
        return DbSet.Any(e => e == entity);
    }
}