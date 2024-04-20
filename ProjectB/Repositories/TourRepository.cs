using ProjectB.Models;

namespace ProjectB.Repositories;

public class TourRepository : AbstractRepository<Tour, int>
{

    private static readonly Lazy<TourRepository> Lazy = new(() => new TourRepository());
    public static TourRepository Instance => Lazy.Value;
    private int _lastId;

    private TourRepository()
    {
        _lastId = Repository.MaxBy(kvp => kvp.Key).Key;
    }

    public int GetNextId()
    {
        return Interlocked.Increment(ref _lastId);
    }

    public IEnumerable<Tour> FindAllToursRemainingToday(Guest guest) =>
        from tour in Repository.Values
        where tour.GetTime() > DateTime.Now 
              && tour.GetParticipants().Count < tour.GetCapacity()
              && !tour.GetParticipants().Contains(guest)
        select tour;
}