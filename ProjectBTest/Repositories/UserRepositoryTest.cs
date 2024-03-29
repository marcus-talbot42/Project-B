using ProjectB.IO;
using ProjectB.Models;
using ProjectB.Repositories;
using ProjectBTest.Fixtures;

namespace ProjectBTest.Repositories;

[TestClass]
public class UserRepositoryTest
{

    private readonly UserRepository _repository = UserRepository.Instance;

    [TestCleanup]
    public void Cleanup()
    {
        _repository.RemoveAll();
    }

    [TestInitialize]
    public void Prepare()
    {
        using (File.Create(".//users.json"))
        {
        }
    }

    [DataTestMethod]
    [DataRow(".//users.json", 1)]
    [DataRow(".//users.json", 10)]
    [DataRow(".//users.json", 100)]
    public void FindAllShouldFindNEntities(string fileName, int amount)
    {
        ICollection<User> users =  UserFixtures.GenerateCollection(amount);
        JsonFileWriter<User> writer = new JsonFileWriter<User>();
        writer.WriteObjects(fileName, users);
        
        _repository.Refresh();
        IEnumerable<User> result = _repository.FindAll();
        
        Assert.AreEqual(amount, result.Count());
        
        File.Delete(fileName);
    }

    [DataTestMethod]
    [DataRow(1)]
    [DataRow(10)]
    [DataRow(100)]
    public void PersistShouldSaveNewUsersToFile(int amount)
    {
        ICollection<User> users = UserFixtures.GenerateCollection(amount);
        foreach (User user in users)
        {
            _repository.Save(user);
        }
        
        Assert.AreEqual(amount, _repository.Count());
        
        _repository.Persist();

        JsonFileReader<User> reader = new JsonFileReader<User>();
        ICollection<User>? result = reader.ReadAllObjects(".//users.json");
        
        Assert.AreEqual(amount, result!.Count);
        foreach (User user in result)
        {
            Assert.AreEqual(_repository.FindById(user.GetId()), user);    
        }
        
        File.Delete(".//users.json");
    }
    
}