using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class TourService(TourRepository repository) : IService<Tour>
{
    public void Create(Tour entity)
    {
        if (repository.Exists(entity)) {
            throw new PrimaryKeyConstraintException("Tour with specified primary key already exists.");
        }
        repository.Save(entity);
    }

    public void Delete(long id)
    {
        repository.Remove(id);
    }
    
    public Tour Read(long id)
    {
        throw new NotImplementedException();
    }

    public void Update(Tour entity, long id)
    {
        repository.Save(entity);
    }

    public IEnumerable<Tour> GetAllToursTodayAfterNow() {
        return repository.GetAllToursTodayAfterNow();
    }
    

    public bool RegisterGuestForTour(Guest guest, Tour tour)
    {
        if (tour.GetRemainingCapacity() == 0)
        {
            return false;
        }
        
        tour.GetParticipants().Add(guest);
        repository.Save(tour);
        return true;
    }
}