using ProjectB.Models;
using System;

namespace ProjectB.Repositories
{
    public class TourRepository : AbstractRepository<Tour, TourCompositeKey>
    {

        public IEnumerable<Tour> GetAllToursTodayAfterNow() =>
            from tour in Repository.Values
            where tour.GetTourTime().Date == DateTime.Today && tour.GetTourTime().CompareTo(DateTime.Now) > 0
            select tour;
    }
}
