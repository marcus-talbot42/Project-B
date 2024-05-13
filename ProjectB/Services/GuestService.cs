using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class GuestService(GuestRepository repository) : IService<Guest, string>
{
    public void Create(Guest entity)
    {
        var guest = entity;
        
        if (!repository.Exists(entity))
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
        repository.Remove(id);
    }

    public Guest FindValidGuestById(string id)
    {
        Guest? guest = repository.FindValidGuestById(id);

        if (guest == null)
        {
            throw new EntityNotFoundException($"Could not find Guest with id: {id}");
        }

        return guest;
    }

    public Guest Read(string read)
    {
        throw new NotImplementedException();
    }
}