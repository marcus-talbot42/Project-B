using ProjectB.Repositories;
using ProjectBTest.Fixtures;

namespace ProjectBTest.Repositories;

[TestClass]
public class TourRepositoryTest
{

    [TestMethod]
    public void Test()
    {
        var repo = new TourRepository();
        var entities = TourFixtures.GenerateCollection(12);
        foreach (var VARIABLE in entities)
        {
            repo.Save(VARIABLE);
        }
        repo.Persist();
    }

}