using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class GuestService(IRepository<Guest, string> repository) : IService<Guest, string>
{
    
    public void Create(Guest entity)
    {
        if (repository.FindById(entity.GetId()) == null)
        {
            repository.Save(entity);
        }
    }

    public void Update(Guest entity, string id)
    {
        repository.Save(entity);
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }
}