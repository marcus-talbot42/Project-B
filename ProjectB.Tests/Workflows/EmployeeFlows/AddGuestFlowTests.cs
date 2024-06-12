using Moq;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Workflows.EmployeeFlows;

namespace ProjectBTest.Workflows.EmployeeFlows
{
    [TestClass]
    public class AddGuestFlowTests
    {
        private Mock<IDatabaseContext> _contextMock;
        private Mock<ITourService> _tourServiceMock;
        private Mock<IGuestService> _guestServiceMock;
        private AddGuestFlow _addGuestFlow;

        [TestInitialize]
        public void TestInitialize()
        {
            _contextMock = new Mock<IDatabaseContext>();
            _tourServiceMock = new Mock<ITourService>();
            _guestServiceMock = new Mock<IGuestService>();

            _addGuestFlow = new AddGuestFlow(
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
            Tour newTour = new Tour(DateTime.Now.AddHours(2)) { Capacity = 13, Departed = false, Participants = new() };

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber))
                .Returns(guest);

            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns((Tour?)null);

            _tourServiceMock.Setup(x => x.RegisterGuestForTour(guest, newTour))
                .Returns(true);

            // Act
            var setGuestResult = _addGuestFlow.SetGuest(ticketNumber);
            var setTourResult = _addGuestFlow.SetTour(newTour);
            var commitResult = _addGuestFlow.Commit();

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
            var setGuestResult = _addGuestFlow.SetGuest(ticketNumber);

            // Assert
            Assert.IsFalse(setGuestResult.Success);
            Assert.AreEqual("guestIsNull", setGuestResult.MessageKey);
        }

        [TestMethod]
        public void InvalidTicket()
        {
            // Arrange
            string ticketNumber = "13548424";

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber))
                .Returns((Guest?)null);

            // Act
            var setGuestResult = _addGuestFlow.SetGuest(ticketNumber);

            // Assert
            Assert.IsFalse(setGuestResult.Success);
            Assert.AreEqual("invalidTicket", setGuestResult.MessageKey);
        }

        [TestMethod]
        public void GuestAlreadyHasReservation()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };
            Tour currentTour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new() { guest.TicketNumber } };

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber))
                .Returns(guest);

            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns(currentTour);

            // Act
            var setGuestResult = _addGuestFlow.SetGuest(ticketNumber);

            // Assert
            Assert.IsFalse(setGuestResult.Success);
            Assert.AreEqual("alreadyHasReservation", setGuestResult.MessageKey);
        }

        [TestMethod]
        public void SelectedTourIsFull()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };
            Tour newTour = new Tour(DateTime.Now.AddHours(2))
            {
                Capacity = 13,
                Departed = false,
                Participants = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13" }
            };

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber))
                .Returns(guest);

            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns((Tour?)null);

            // Act
            var setGuestResult = _addGuestFlow.SetGuest(ticketNumber);
            var setTourResult = _addGuestFlow.SetTour(newTour);

            // Assert
            Assert.IsTrue(setGuestResult.Success);
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourFull", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SelectedTourIsInPast()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };
            Tour newTour = new Tour(DateTime.Now.AddHours(-1)) { Capacity = 13, Departed = false, Participants = new() };

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber))
                .Returns(guest);

            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns((Tour?)null);

            // Act
            var setGuestResult = _addGuestFlow.SetGuest(ticketNumber);
            var setTourResult = _addGuestFlow.SetTour(newTour);

            // Assert
            Assert.IsTrue(setGuestResult.Success);
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourInPast", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SelectedTourHasDeparted()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };
            Tour newTour = new Tour(DateTime.Now.AddHours(2)) { Capacity = 13, Departed = true, Participants = new() };

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber))
                .Returns(guest);

            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns((Tour?)null);

            // Act
            var setGuestResult = _addGuestFlow.SetGuest(ticketNumber);
            var setTourResult = _addGuestFlow.SetTour(newTour);

            // Assert
            Assert.IsTrue(setGuestResult.Success);
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourDeparted", setTourResult.MessageKey);
        }

        [TestMethod]
        public void CommitWithoutSettingGuest()
        {
            // Arrange
            Tour newTour = new Tour(DateTime.Now.AddHours(2)) { Capacity = 13, Departed = false, Participants = new() };

            _addGuestFlow.SetTour(newTour);

            // Act
            var commitResult = _addGuestFlow.Commit();

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

            _addGuestFlow.SetGuest(ticketNumber);

            // Act
            var commitResult = _addGuestFlow.Commit();

            // Assert
            Assert.IsFalse(commitResult.Success);
            Assert.AreEqual("tourIsNull", commitResult.MessageKey);
        }
    }
}
