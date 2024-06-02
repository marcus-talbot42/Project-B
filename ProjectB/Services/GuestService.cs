using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class GuestService(IGuestRepository repository) : AbstractService<Guest>(repository), IGuestService
{
    public Guest? FindValidGuestById(string username) => repository.FindValidGuestByUsername(username);
}