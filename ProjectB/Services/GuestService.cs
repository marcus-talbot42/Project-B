using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class GuestService(IGuestRepository repository) : AbstractService<Guest>(repository), IGuestService
{
    public Guest FindValidGuestById(string username)
    {
        Guest? guest = repository.FindValidGuestByUsername(username);

        if (guest == null)
        {
            throw new EntityNotFoundException($"Could not find Guest with ticketnumber: {username}");
        }

        return guest;
    }
}