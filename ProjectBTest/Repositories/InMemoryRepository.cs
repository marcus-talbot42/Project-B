using ProjectB.Repositories;
public class InMemoryRepository<T, TKey> : IRepository<T, TKey>
{
    private T entity;

    public InMemoryRepository(T entity)
    {
        this.entity = entity;
    }

    public void Save(T entity)
    {
        this.entity = entity;
    }

    public void Remove(TKey id)
    {
        // Do nothing
    }

    public T FindById(TKey id)
    {
        return entity;
    }
}