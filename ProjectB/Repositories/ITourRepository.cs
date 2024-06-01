using ProjectB.Models;

namespace ProjectB.Repositories
{
    public interface ITourRepository : IRepository<Tour>
    {
        IEnumerable<Tour> GetAllToursTodayAfterNow();
    }
}