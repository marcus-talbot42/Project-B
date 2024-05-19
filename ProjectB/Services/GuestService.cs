using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class GuestService(GuestRepository repository) : IService<Guest, long>
{
    public void Create(Guest entity)
    {
        var guest = entity;
        
        if (!repository.Exists(entity))
        {
            repository.Save(entity);
        }
    }

    public void Update(Guest entity, long id)
    {
        repository.Save(entity);
    }

    public void Delete(long id)
    {
        repository.Remove(id);
    }

    public Guest FindValidGuestById(string username)
    {
        Guest? guest = repository.FindValidGuestByUsername(username);

        if (guest == null)
        {
            throw new EntityNotFoundException($"Could not find Guest with ticketnumber: {username}");
        }

        return guest;
    }

    public Guest Read(long read)
    {
        throw new NotImplementedException();
    }
}