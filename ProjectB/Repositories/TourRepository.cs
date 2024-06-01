using ProjectB.Database;
using ProjectB.Models;

namespace ProjectB.Repositories
{
    public class TourRepository(DatabaseContext context) : AbstractRepository<Tour>(context)
    {
        public IEnumerable<Tour> GetAllToursTodayAfterNow() =>
            from tour in DbSet
            where tour.Start.Date == DateTime.Today && tour.Start >= DateTime.Now
            select tour;
    }
}