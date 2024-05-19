using ProjectB.Database;
using ProjectB.Models;

namespace ProjectB.Repositories;

public class GuestRepository(DatabaseContext context) : AbstractRepository<Guest>(context)
{
    public Guest? FindValidGuestByUsername(string username)
    {
        return DbSet.ToList().FirstOrDefault(guest => guest.Username == username && guest.ValidDate == DateOnly.FromDateTime(DateTime.Now));
    }

    
}