using ProjectB.Database;
using ProjectB.Models;

namespace ProjectB.Repositories
{
    public class TourRepository(IDatabaseContext context) : AbstractRepository<Tour>(context), ITourRepository
    {
        public IEnumerable<Tour> GetAllToursTodayAfterNow() =>
            from tour in DbSet
            where tour.Start.Date == DateTime.Today && tour.Start >= DateTime.Now
            select tour;

        public Tour? GetTourForGuest(Guest guest)
             => DbSet.FirstOrDefault(tour => tour.Participants.Contains(guest.TicketNumber));
    }
}