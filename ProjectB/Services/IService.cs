using ProjectB.Models;

namespace ProjectB.Services;

public interface IService<TEntity>
    where TEntity : IEntity
{

    void Create(TEntity entity);

    public void Update(TEntity entity) => Update(entity, entity.GetId());

    void Update(TEntity entity, long id);

    void Delete(long id);

    void Delete(TEntity entity) => Delete(entity.GetId());

    TEntity Read(long read);

}