using ProjectB.IO;
using ProjectB.Models;
using ProjectB.Repositories;
using ProjectBTest.Fixtures;

namespace ProjectBTest.Repositories;

[TestClass]
public class UserRepositoryTest
{

    private readonly EmployeeRepository _repository = EmployeeRepository.Instance;

    [TestCleanup]
    public void Cleanup()
    {
        _repository.RemoveAll();
    }

    [TestInitialize]
    public void Prepare()
    {
        using (File.Create(_repository.GetFileLocation()))
        {
        }
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(10)]
    [DataRow(100)]
    public void FindAllShouldFindNEntities(int amount)
    {
        ICollection<Employee> users = EmployeeFixtures.GenerateCollection(amount);
        JsonFileWriter<Employee> writer = new();
        writer.WriteObjects(_repository.GetFileLocation(), users);

        _repository.Refresh();
        IEnumerable<Employee> result = _repository.FindAll();

        Assert.AreEqual(amount, result.Count());

        File.Delete(_repository.GetFileLocation());
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(10)]
    [DataRow(100)]
    public void PersistShouldSaveNewUsersToFile(int amount)
    {
        ICollection<Employee> users = EmployeeFixtures.GenerateCollection(amount);
        foreach (Employee user in users)
        {
            _repository.Save(user);
        }

        Assert.AreEqual(amount, _repository.Count());

        _repository.Persist();

        JsonFileReader<Employee> reader = new();
        ICollection<Employee>? result = reader.ReadAllObjects(_repository.GetFileLocation());

        Assert.AreEqual(amount, result!.Count);
        foreach (Employee user in result)
        {
            Assert.AreEqual(_repository.FindById(user.GetId()), user);
        }

        File.Delete(_repository.GetFileLocation());
    }

}