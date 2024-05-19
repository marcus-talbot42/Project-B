using ProjectB.Models;

namespace ProjectB.Repositories;

public class GuestRepository : AbstractRepository<Guest, string>
{
    public Guest? FindValidGuestById(string id)
    {
        return Repository.Values
            .FirstOrDefault(guest => guest.GetId() == id && guest.IsValid());
    }

    
}