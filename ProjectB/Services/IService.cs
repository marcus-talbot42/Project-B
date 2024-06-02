using ProjectB.Models;

namespace ProjectB.Services;

public interface IService<T> where T : AbstractEntity
{

    void Add(T entity);

    void AddRange(List<T> range);

    void Delete(long id);

    void Delete(T entity) => Delete(entity.Id);

    int SaveChanges();
}