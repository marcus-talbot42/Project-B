using ProjectB.Services;

namespace ProjectBTest.Services;

[TestClass]
public class TourServiceTest
{
    
    // private TourService _service;
    //
    // [TestMethod]
    // public void TestCreate()
    // {
    //     _repositoryMock = new TourRepositoryMock();
    //     _service = new TourService(_repositoryMock);
    //     var tour = new Tour(25, DateTime.Now.AddDays(20), new List<Guest>());
    //     Assert.IsTrue(_repositoryMock.Count() == 0);
    //     
    //     _service.Create(tour);
    //     Assert.IsTrue(_repositoryMock.Count() == 1);
    // }
    //
    // [TestMethod]
    // public void TestCreateFailOnPrimaryKeyAlreadyExists()
    // {
    //     _repositoryMock = new TourRepositoryMock();
    //     _service = new TourService(_repositoryMock);
    //     var tour = new Tour(25, DateTime.Now.AddDays(20), new List<Guest>());
    //     Assert.IsTrue(_repositoryMock.Count() == 0);
    //
    //     _service.Create(tour);
    //     Assert.IsTrue(_repositoryMock.Count() == 1);
    //     Assert.ThrowsException<PrimaryKeyConstraintException>(() => _service.Create(tour));
    // }
}