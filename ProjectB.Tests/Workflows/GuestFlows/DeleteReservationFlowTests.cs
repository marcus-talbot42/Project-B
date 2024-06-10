using Moq;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Workflows.GuestFlows;

namespace ProjectBTest.Workflows
{
    [TestClass]
    public class DeleteReservationFlowTests
    {
        private Mock<IDatabaseContext> _contextMock;
        private Mock<ITourService> _tourServiceMock;
        private DeleteReservationFlow _deleteReservationFlow;

        [TestInitialize]
        public void TestInitialize()
        {
            _contextMock = new Mock<IDatabaseContext>();
            _tourServiceMock = new Mock<ITourService>();

            _deleteReservationFlow = new DeleteReservationFlow(
                _contextMock.Object,
                _tourServiceMock.Object);
        }

        [TestMethod]
        public void HappyFlow()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new() { guest.TicketNumber } };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.DeleteReservationGuest(guest))
                .Returns(true);

            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns(tour);

            // Act  
            var setGuest = _deleteReservationFlow.SetGuest(guest);
            var commit = _deleteReservationFlow.Commit();

            // Assert  
            Assert.IsTrue(setGuest.Success);
            Assert.IsTrue(commit.Success);
        }

        [TestMethod]
        public void SetGuest_GuestIsNull()
        {
            // Arrange  
            Guest? guest = null;

            // Act  
            var setGuest = _deleteReservationFlow.SetGuest(guest);

            // Assert  
            Assert.IsFalse(setGuest.Success);
            Assert.AreEqual("guestIsNull", setGuest.MessageKey);
        }

        [TestMethod]
        public void SetGuest_NoReservationFound()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns((Tour?)null);

            // Act  
            var setGuest = _deleteReservationFlow.SetGuest(guest);

            // Assert  
            Assert.IsFalse(setGuest.Success);
            Assert.AreEqual("noReservationFound", setGuest.MessageKey);
        }

        [TestMethod]
        public void SetGuest_TourDeparted()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = true, Participants = new() { guest.TicketNumber } };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns(tour);

            // Act  
            var setGuest = _deleteReservationFlow.SetGuest(guest);

            // Assert  
            Assert.IsFalse(setGuest.Success);
            Assert.AreEqual("tourDeparted", setGuest.MessageKey);
        }

        [TestMethod]
        public void Commit_GuestIsNull()
        {
            // Act  
            var commit = _deleteReservationFlow.Commit();

            // Assert  
            Assert.IsFalse(commit.Success);
            Assert.AreEqual("guestIsNull", commit.MessageKey);
        }

        [TestMethod]
        public void Commit_TourIsNull()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns((Tour?)null);

            // Act  
            _deleteReservationFlow.SetGuest(guest);
            var commit = _deleteReservationFlow.Commit();

            // Assert  
            Assert.IsFalse(commit.Success);
            Assert.AreEqual("guestIsNull", commit.MessageKey);
        }
    }
}