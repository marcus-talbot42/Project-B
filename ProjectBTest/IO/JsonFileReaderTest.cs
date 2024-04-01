using ProjectB.IO;
using ProjectB.Models;
using ProjectBTest.Fixtures;

namespace ProjectBTest.IO;

[TestClass]
public class JsonFileReaderTest
{
    
    [DataTestMethod]
    [DataRow(".//test.json", 1)]
    [DataRow(".//test.json", 10)]
    [DataRow(".//test.json", 100)]
    public void ShouldJsonToFileWhenPassedAListOfUsers(string fileName, int amount)
    {
        ICollection<Employee> userList = EmployeeFixtures.GenerateCollection(amount);
        JsonFileWriter<Employee> writer = new();
        writer.WriteObjects(fileName, userList);

        JsonFileReader<Employee> reader = new();
        ICollection<Employee>? result = reader.ReadAllObjects(fileName);
        Assert.IsNotNull(result);
        
        Assert.AreEqual(amount, result.Count);
        foreach (var user in result)
        {
            Assert.IsTrue(userList.Contains(user));
        }
        
        File.Delete(fileName);
    }
}