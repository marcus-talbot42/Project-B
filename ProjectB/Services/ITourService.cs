using ProjectB.Models;

namespace ProjectB.Services;

public interface ITourService : IService<Tour>
{
    IEnumerable<Tour> GetAllToursTodayAfterNow();

    bool RegisterGuestForTour(Guest guest, Tour tour);

    int GetRemainingCapacity(Tour tour);
}