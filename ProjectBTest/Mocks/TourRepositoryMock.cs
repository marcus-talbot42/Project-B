using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectBTest.Mocks;

public class TourRepositoryMock : IRepository<Tour, TourCompositeKey>
{

    private Dictionary<int, Tour> _repository = new Dictionary<int, Tour>();
    private List<Tour> _fileSystem = new List<Tour>();


    public void Save(Tour entity)
    {
        throw new NotImplementedException();
    }

    public Tour? FindById(TourCompositeKey id)
    {
        throw new NotImplementedException();
    }

    public Tour? FindById(int id)
    {
        return _repository[id];
    }

    public IEnumerable<Tour> FindAll()
    {
        return _repository.Values;
    }

    public void Remove(TourCompositeKey id)
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        _repository.Remove(id);
    }

    public void RemoveAll()
    {
        _repository.Clear();
    }

    public void Refresh()
    {
        // _repository = new Dictionary<int, Tour>()
        // {
        //     {1, new Tour(1, DateTime.Now, new List<Guest>())},
        //     {2, new Tour(2, DateTime.Now.AddDays(1), new List<Guest>())},
        //     {3, new Tour(3, DateTime.Now.AddDays(2), new List<Guest>())}
        // };
    }

    public void Persist()
    {
        _fileSystem.Clear();
        foreach (var VARIABLE in _repository.Values)
        {
            _fileSystem.Add(VARIABLE);       
        }
    }

    public int Count()
    {
        return _repository.Values.Count;
    }

    public string GetFileLocation()
    {
        return ".//TourRepository.json";
    }

    public bool Exists(int id)
    {
        return _repository.ContainsKey(id);
    }
}