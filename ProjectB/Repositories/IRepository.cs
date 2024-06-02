using ProjectB.Models;

namespace ProjectB.Repositories;

/**
 * Base for every repository used in this program.
 */
public interface IRepository<T>
    where T : AbstractEntity
{
    void Add(T entity);

    void AddRange(List<T> range);

    T? Find(long id);

    IEnumerable<T> FindAll();

    void Remove(long id);

    void Remove(T entity) => Remove(entity.Id);

    void RemoveAll();

    int Count();

    int SaveChanges();

}