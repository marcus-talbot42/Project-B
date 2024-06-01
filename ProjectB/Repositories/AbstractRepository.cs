using Microsoft.EntityFrameworkCore;
using ProjectB.Database;
using ProjectB.Models;

namespace ProjectB.Repositories;

public abstract class AbstractRepository<T>(DatabaseContext databaseContext) : IRepository<T> where T : AbstractEntity
{

    protected DbSet<T> DbSet { get; } = databaseContext.GetRelevantDbSet<T>()!;

    public void Add(T entity) => DbSet.Add(entity: entity);

    public T? Find(long id) => DbSet.Find(id);

    public IEnumerable<T> FindAll() => DbSet.ToList();

    public void Remove(long id) => DbSet.Remove(DbSet.Find(id)!);

    public void RemoveAll() => DbSet.RemoveRange(DbSet.ToArray());

    public int Count() => DbSet.Count();

    public bool Exists(T entity) => DbSet.Any(e => e == entity);

    public int SaveChanges() => databaseContext.SaveChanges();
}