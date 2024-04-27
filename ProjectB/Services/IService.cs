using ProjectB.Models;

namespace ProjectB.Services;

public interface IService<TEntity, TId>
    where TEntity : IEntity<TId>
{

    void Create(TEntity entity);

    public void Update(TEntity entity) => Update(entity, entity.GetId());

    void Update(TEntity entity, TId id);

    void Delete(TId id);

    void Delete(TEntity entity) => Delete(entity.GetId());

    TEntity Read(TId read);

}