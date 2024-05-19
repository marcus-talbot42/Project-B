using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public interface ITourService : IService<Tour>
{
    IEnumerable<Tour> GetAllToursTodayAfterNow();

    bool RegisterGuestForTour(Guest guest, Tour tour);

    int GetRemainingCapacity(Tour tour);
}