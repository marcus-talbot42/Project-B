using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class GuestService : IService<Guest, string>
{

    private static readonly Lazy<GuestService> Lazy = new(() => new GuestService());
    public static GuestService Instance => Lazy.Value;
    
    private readonly GuestRepository _repository = GuestRepository.Instance;
    
    public void Create(Guest entity)
    {
        if (_repository.FindById(entity.GetId()) == null)
        {
            _repository.Save(entity);
        }
    }

    public void Update(Guest entity, string id)
    {
        _repository.Save(entity);
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public Guest? FindValidGuestById(string id)
    {
        if (!_repository.Exists(id))
        {
            return null;
        }

        Guest guest = _repository.FindById(id);
        return !guest.IsValid() ? null : guest;
    }
}