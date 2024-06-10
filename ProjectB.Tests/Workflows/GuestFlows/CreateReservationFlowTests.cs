using Moq;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Workflows.GuestFlows;

namespace ProjectBTest.Workflows.GuestFlows
{
    [TestClass]
    public class CreateReservationFlowTests
    {
        private Mock<IDatabaseContext> _contextMock;
        private Mock<ITourService> _tourServiceMock;
        private CreateReservationFlow _createReservationFlow;

        [TestInitialize]
        public void TestInitialize()
        {
            _contextMock = new Mock<IDatabaseContext>();
            _tourServiceMock = new Mock<ITourService>();

            _createReservationFlow = new CreateReservationFlow(
                _contextMock.Object,
                _tourServiceMock.Object);
        }

        [TestMethod]
        public void HappyFlow()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new() };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns((Tour?)null);

            _tourServiceMock.Setup(x => x.RegisterGuestForTour(guest, tour))
                .Returns(true);

            // Act  
            var setGuestResult = _createReservationFlow.SetGuest(guest);
            var setTourResult = _createReservationFlow.SetTour(tour);
            var commitResult = _createReservationFlow.Commit();

            // Assert  
            Assert.IsTrue(setGuestResult.Success);
            Assert.IsTrue(setTourResult.Success);
            Assert.IsTrue(commitResult.Success);
        }

        [TestMethod]
        public void SetGuest_GuestIsNull()
        {
            // Act  
            var setGuestResult = _createReservationFlow.SetGuest(null);

            // Assert  
            Assert.IsFalse(setGuestResult.Success);
            Assert.AreEqual("guestIsNull", setGuestResult.MessageKey);
        }

        [TestMethod]
        public void SetGuest_AlreadyHasReservation()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new() { "13548424" } };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns(tour);

            // Act  
            var setGuestResult = _createReservationFlow.SetGuest(guest);

            // Assert  
            Assert.IsFalse(setGuestResult.Success);
            Assert.AreEqual("alreadyHasReservation", setGuestResult.MessageKey);
        }

        [TestMethod]
        public void SetTour_TourIsNull()
        {
            // Act  
            var setTourResult = _createReservationFlow.SetTour(null);

            // Assert  
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourIsNull", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SetTour_TourFull()
        {
            // Arrange  
            Tour tour = new Tour(DateTime.Now.AddHours(1))
            {
                Capacity = 13,
                Departed = false,
                Participants = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13" }
            };

            // Act  
            var setTourResult = _createReservationFlow.SetTour(tour);

            // Assert  
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourFull", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SetTour_TourInPast()
        {
            // Arrange  
            Tour tour = new Tour(DateTime.Now.AddHours(-1)) { Capacity = 13, Departed = false, Participants = new() };

            // Act  
            var setTourResult = _createReservationFlow.SetTour(tour);

            // Assert  
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourInPast", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SetTour_TourDeparted()
        {
            // Arrange  
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = true, Participants = new() };

            // Act  
            var setTourResult = _createReservationFlow.SetTour(tour);

            // Assert  
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourDeparted", setTourResult.MessageKey);
        }

        [TestMethod]
        public void Commit_GuestIsNull()
        {
            // Arrange
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new() };

            // Act  
            var setTourResult = _createReservationFlow.SetTour(tour);
            var commitResult = _createReservationFlow.Commit();

            // Assert  
            Assert.IsTrue(setTourResult.Success);
            Assert.IsFalse(commitResult.Success);
            Assert.AreEqual("guestIsNull", commitResult.MessageKey);
        }

        [TestMethod]
        public void Commit_TourIsNull()
        {
            // Arrange
            Guest guest = new Guest() { TicketNumber = "13548424" };

            // Act  
            var setGuestResult = _createReservationFlow.SetGuest(guest);
            var commitResult = _createReservationFlow.Commit();

            // Assert  
            Assert.IsTrue(setGuestResult.Success);
            Assert.IsFalse(commitResult.Success);
            Assert.AreEqual("tourIsNull", commitResult.MessageKey);
        }
    }
}
