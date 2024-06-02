using ProjectB.Database;
using ProjectB.Models;

namespace ProjectB.Repositories;

public class GuestRepository(IDatabaseContext context) : AbstractRepository<Guest>(context), IGuestRepository
{
    public Guest? FindValidGuestByUsername(string username)
    {
        return DbSet.ToList().FirstOrDefault(guest => guest.TicketNumber == username && (guest.ValidDate == DateOnly.FromDateTime(DateTime.Now) || !guest.Expires));
    }
}