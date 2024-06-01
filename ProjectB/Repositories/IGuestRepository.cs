using ProjectB.Models;

namespace ProjectB.Repositories
{
    public interface IGuestRepository : IRepository<Guest>
    {
        Guest? FindValidGuestByUsername(string username);
    }
}