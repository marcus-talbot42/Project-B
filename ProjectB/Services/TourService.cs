using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class TourService(TourRepository repository) : IService<Tour, TourCompositeKey>
{
    public void Create(Tour entity)
    {
        if (repository.Exists(entity)) {
            throw new PrimaryKeyConstraintException("Tour with specified primary key already exists.");
        }
        repository.Save(entity);
    }

    public void Delete(TourCompositeKey id)
    {
        throw new NotImplementedException();
    }

    public void Update(Tour entity, TourCompositeKey id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Tour> GetAllToursTodayAfterNow() {
        return repository.GetAllToursTodayAfterNow();
    }

    public bool RegisterUserForTour(Tour tour, Guest guest) {
        if (tour.RemainingCapacity == 0 || guest.IsGuestInTour)
        {
            return false;
        }

        guest.Tour = tour.GetId();
        tour.GetParticipants().Add(guest.GetId());
        repository.Save(tour);

        return true;
    }
}