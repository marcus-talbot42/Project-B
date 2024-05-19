using ProjectB.Models;
using System;
using ProjectB.Database;

namespace ProjectB.Repositories
{
    public class TourRepository(DatabaseContext context) : AbstractRepository<Tour, long>(context)
    {
        public IEnumerable<Tour> GetAllToursTodayAfterNow() =>
            from tour in DbSet
            where tour.GetTourTime().Date == DateTime.Today && tour.GetTourTime().CompareTo(DateTime.Now) > 0
            select tour;
    }
}