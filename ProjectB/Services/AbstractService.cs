using NuGet.Protocol.Core.Types;
using ProjectB.Models;
using ProjectB.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public abstract class AbstractService<T>(AbstractRepository<T> repository) : IService<T> where T : AbstractEntity
    {
        private AbstractRepository<T> Repository { get; } = repository;

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
