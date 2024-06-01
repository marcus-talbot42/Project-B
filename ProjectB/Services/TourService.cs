using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class TourService(ITourRepository repository) : AbstractService<Tour>(repository), ITourService
{
    public static readonly int MaxCapacity = 13;
    public static readonly int TourDuration = 40;
    public static readonly int DefaultTourInterval = 20;

    public IEnumerable<Tour> GetAllToursTodayAfterNow()
    {
        return repository.GetAllToursTodayAfterNow();
    }

    public bool RegisterGuestForTour(Guest guest, Tour tour)
    {
        if (GetRemainingCapacity(tour) == 0)
        {
            return false;
        }

        tour.Participants.Add(guest);

        SaveChanges();

        return true;
    }

    public int GetRemainingCapacity(Tour tour)
    {
        return MaxCapacity - tour.Participants.Count;
    }
}