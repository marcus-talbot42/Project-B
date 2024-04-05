using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class GuestService : IService<Guest, string>
{

    private static readonly Lazy<GuestService> Lazy = new(() => new GuestService());
    public static GuestService Instance => Lazy.Value;
    
    private static readonly GuestRepository Repository = GuestRepository.Instance;
    
    public void Create(Guest entity)
    {
        if (Repository.FindById(entity.GetId()) == null)
        {
            Repository.Save(entity);
        }
    }

    public void Update(Guest entity, string id)
    {
        Repository.Save(entity);
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }
}