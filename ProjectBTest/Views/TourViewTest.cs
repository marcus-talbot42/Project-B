using ProjectB.Repositories;
using ProjectBTest.Fixtures;

namespace ProjectBTest.Views;

[TestClass]
public class TourViewTest
{

    [TestMethod]
    public void Test()
    {
        TourRepository repository = new();
        var tours = TourFixtures.GenerateCollection(100);
    }

}