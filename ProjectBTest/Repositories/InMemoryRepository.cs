using ProjectB.Models;
using ProjectB.Repositories;
using System.Collections.Generic;

public class InMemoryRepository<T, TKey> : IRepository<T, TKey> where T : IEntity<TKey> where TKey : class
{
    private List<T> entities = new List<T>();

    public void Save(T entity)
    {
        entities.Add(entity);
    }

    public void Remove(TKey id)
    {
        entities.RemoveAll(e => e.GetId().Equals(id));
    }
    
    public T FindById(TKey id)
    {
        return entities.Find(e => e.GetId().Equals(id)) ?? default(T);
    }

    public IEnumerable<T> FindAll()
    {
        return entities;
    }

    public void RemoveAll()
    {
        entities.Clear();
    }

    public void Refresh()
    {
        // Implement this method if needed
    }

    public void Persist()
    {
        // Implement this method if needed
    }

    public int Count()
    {
        return entities.Count;
    }
}