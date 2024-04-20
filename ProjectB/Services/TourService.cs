using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;
using ProjectB.settings;

namespace ProjectB.Services;

public class TourService : IService<Tour, int>
{

    private static readonly Lazy<TourService> Lazy = new(() => new TourService());
    public static TourService Instance => Lazy.Value;

    private readonly Lazy<TourRepository> _repository = new (TourRepository.Instance);
    
    public void Create(Tour entity)
    {
        if (entity.GetId() == 0)
        {
            entity = new(_repository.Value.GetNextId(), entity.GetTime(), entity.GetLocation(), entity.GetCapacity(), entity.GetParticipants());
            _repository.Value.Save(entity);
        }

        if (!_repository.Value.Exists(entity.GetId()))
        {
            _repository.Value.Save(entity);
        }

        throw new PrimaryKeyConstraintException($"Could not save Tour, as the Primary Key is already taken: {entity.GetId()}");
    }

    public void Update(Tour entity, int id)
    {
        if (id == 0)
        {
            throw new NonNullConstraintException("Could not update Tour. Primary Key was not set.");
        }

        if (!_repository.Value.Exists(entity.GetId()))
        {
            throw new InvalidEntityException($"Could not update Tour with id {entity.GetId()}, as no entity with that ID is managed by the repository.");
        }
        
        _repository.Value.Save(entity);
    }

    public void Delete(int id)
    {
        _repository.Value.Remove(id);
    }

    public IEnumerable<Tour> FindAllRemainingToursToday()
    {
        AbstractUser? currentUser = Settings.GetCurrentUser();
        if (currentUser == null || currentUser.GetType() != typeof(Guest) || !((Guest) currentUser).IsValid())
        {
            throw new InvalidSessionException($"User is either not logged in, or not a Guest.");
        }
        return _repository.Value.FindAllToursRemainingToday((Guest) currentUser);
    }

    public void AddReservation(Tour tour)
    {
        AbstractUser? currentUser = Settings.GetCurrentUser();
        if (currentUser == null || currentUser.GetType() != typeof(Guest) || !((Guest) currentUser).IsValid())
        {
            throw new InvalidSessionException("User is either not logged in, does not have a ticket for today, or is not a Guest.");
        }
        ((Guest) currentUser).SetTour(tour);
        tour.GetParticipants().Add((Guest) currentUser);
    }
}