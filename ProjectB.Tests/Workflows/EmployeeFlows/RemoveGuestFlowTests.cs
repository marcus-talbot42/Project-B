using Moq;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Workflows.EmployeeFlows;

namespace ProjectBTest.Workflows.EmployeeFlows
{
    [TestClass]
    public class RemoveGuestFlowTests
    {
        private Mock<IDatabaseContext> _contextMock;
        private Mock<ITourService> _tourServiceMock;
        private Mock<IGuestService> _guestServiceMock;
        private RemoveGuestFlow _removeGuestFlow;

        [TestInitialize]
        public void TestInitialize()
        {
            _contextMock = new Mock<IDatabaseContext>();
            _tourServiceMock = new Mock<ITourService>();
            _guestServiceMock = new Mock<IGuestService>();

            _removeGuestFlow = new RemoveGuestFlow(
                _contextMock.Object,
                _tourServiceMock.Object,
                _guestServiceMock.Object);
        }

        [TestMethod]
        public void HappyFlow()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };
            Tour currentTour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new() { guest.TicketNumber } };

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber))
                .Returns(guest);

            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns(currentTour);

            _tourServiceMock.Setup(x => x.DeleteReservationGuest(guest))
                .Returns(true);

            // Act
            var setGuestResult = _removeGuestFlow.SetGuest(ticketNumber);
            var setTourResult = _removeGuestFlow.SetTour(currentTour);
            var commitResult = _removeGuestFlow.Commit();

            // Assert
            Assert.IsTrue(setGuestResult.Success);
            Assert.IsTrue(setTourResult.Success);
            Assert.IsTrue(commitResult.Success);
        }

        [TestMethod]
        public void GuestIsNullOrWhiteSpace()
        {
            // Arrange
            string ticketNumber = "   ";

            // Act
            var setGuestResult = _removeGuestFlow.SetGuest(ticketNumber);

            // Assert
            Assert.IsFalse(setGuestResult.Success);
            Assert.AreEqual("guestIsNull", setGuestResult.MessageKey);
        }

        [TestMethod]
        public void InvalidGuestTicket()
        {
            // Arrange
            string ticketNumber = "13548424";

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber))
                .Returns((Guest?)null);

            // Act
            var setGuestResult = _removeGuestFlow.SetGuest(ticketNumber);

            // Assert
            Assert.IsFalse(setGuestResult.Success);
            Assert.AreEqual("guestIsNull", setGuestResult.MessageKey);
        }

        [TestMethod]
        public void NoReservationFound()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber))
                .Returns(guest);

            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns((Tour?)null);

            // Act
            var setGuestResult = _removeGuestFlow.SetGuest(ticketNumber);

            // Assert
            Assert.IsFalse(setGuestResult.Success);
            Assert.AreEqual("noReservationFound", setGuestResult.MessageKey);
        }

        [TestMethod]
        public void SelectedTourIsEmpty()
        {
            // Arrange
            Tour tour = new Tour(DateTime.Now.AddHours(2)) { Capacity = 13, Departed = false, Participants = new() };

            // Act
            var setTourResult = _removeGuestFlow.SetTour(tour);

            // Assert
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourEmpty", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SelectedTourIsInPast()
        {
            // Arrange
            Tour tour = new Tour(DateTime.Now.AddHours(-1)) { Capacity = 13, Departed = false, Participants = new() { "13548424" } };

            // Act
            var setTourResult = _removeGuestFlow.SetTour(tour);

            // Assert
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourInPast", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SelectedTourHasDeparted()
        {
            // Arrange
            Tour tour = new Tour(DateTime.Now.AddHours(2)) { Capacity = 13, Departed = true, Participants = new() { "13548424" } };

            // Act
            var setTourResult = _removeGuestFlow.SetTour(tour);

            // Assert
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourDeparted", setTourResult.MessageKey);
        }

        [TestMethod]
        public void CommitWithoutSettingGuest()
        {
            // Arrange
            Tour tour = new Tour(DateTime.Now.AddHours(2)) { Capacity = 13, Departed = false, Participants = new() { "13548424" } };

            _removeGuestFlow.SetTour(tour);

            // Act
            var commitResult = _removeGuestFlow.Commit();

            // Assert
            Assert.IsFalse(commitResult.Success);
            Assert.AreEqual("guestIsNull", commitResult.MessageKey);
        }

        [TestMethod]
        public void CommitWithoutSettingTour()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber))
                .Returns(guest);

            _removeGuestFlow.SetGuest(ticketNumber);

            // Act
            var commitResult = _removeGuestFlow.Commit();

            // Assert
            Assert.IsFalse(commitResult.Success);
            Assert.AreEqual("tourIsNull", commitResult.MessageKey);
        }
    }
}
