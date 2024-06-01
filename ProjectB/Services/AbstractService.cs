using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services
{
    public abstract class AbstractService<T>(IRepository<T> repository) : IService<T> where T : AbstractEntity
    {
        private IRepository<T> Repository { get; } = repository;

        public void Add(T entity)
        {
            Repository.Add(entity);
        }

        public void Delete(long id)
        {
            Repository.Remove(id);
        }

        public int SaveChanges()
        {
            return Repository.SaveChanges();
        }
    }
}
