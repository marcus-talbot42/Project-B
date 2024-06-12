using Moq;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Workflows.EmployeeFlows;

namespace ProjectBTest.Workflows.EmployeeFlows
{
    [TestClass]
    public class StartTourFlowTests
    {
        private Mock<IDatabaseContext> _contextMock;
        private Mock<ITourService> _tourServiceMock;
        private Mock<IGuestService> _guestServiceMock;
        private Mock<IEmployeeService> _employeeServiceMock;
        private StartTourFlow _startTourFlow;

        [TestInitialize]
        public void TestInitialize()
        {
            _contextMock = new Mock<IDatabaseContext>();
            _tourServiceMock = new Mock<ITourService>();
            _guestServiceMock = new Mock<IGuestService>();
            _employeeServiceMock = new Mock<IEmployeeService>();

            _startTourFlow = new StartTourFlow(
                _contextMock.Object,
                _tourServiceMock.Object,
                _guestServiceMock.Object,
                _employeeServiceMock.Object);
        }

        [TestMethod]
        public void HappyFlow()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new List<string> { guest.TicketNumber } };
            string employeeNumber = "001";

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber)).Returns(guest);
            _employeeServiceMock.Setup(x => x.ValidateEmployeeNumber(employeeNumber)).Returns(true);

            // Act
            var setTourResult = _startTourFlow.SetTour(tour);
            var setEmployeeNumberResult = _startTourFlow.SetEmployeeNumber(employeeNumber);
            var addGuestResult = _startTourFlow.AddGuest(ticketNumber);
            var commitResult = _startTourFlow.Commit();

            // Assert
            Assert.IsTrue(setTourResult.Success);
            Assert.IsTrue(setEmployeeNumberResult.Success);
            Assert.IsTrue(addGuestResult.Success);
            Assert.IsTrue(commitResult.Success);
        }

        [TestMethod]
        public void AddGuest_GuestIsNullOrWhiteSpace()
        {
            // Act
            var addGuestResult = _startTourFlow.AddGuest("   ");

            // Assert
            Assert.IsFalse(addGuestResult.Success);
            Assert.AreEqual("guestIsNull", addGuestResult.MessageKey);
        }

        [TestMethod]
        public void AddGuest_InvalidTicket()
        {
            // Arrange
            string ticketNumber = "13548424";

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber)).Returns((Guest?)null);

            // Act
            var addGuestResult = _startTourFlow.AddGuest(ticketNumber);

            // Assert
            Assert.IsFalse(addGuestResult.Success);
            Assert.AreEqual("invalidTicket", addGuestResult.MessageKey);
        }

        [TestMethod]
        public void AddGuest_TicketAlreadyScanned()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber)).Returns(guest);

            // Act
            _startTourFlow.ScannedTickets.Add(ticketNumber);
            var addGuestResult = _startTourFlow.AddGuest(ticketNumber);

            // Assert
            Assert.IsFalse(addGuestResult.Success);
            Assert.AreEqual("ticketAlreadyScanned", addGuestResult.MessageKey);
        }

        [TestMethod]
        public void AddGuest_TourIsNull()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber)).Returns(guest);

            // Act
            var addGuestResult = _startTourFlow.AddGuest(ticketNumber);

            // Assert
            Assert.IsFalse(addGuestResult.Success);
            Assert.AreEqual("tourIsNull", addGuestResult.MessageKey);
        }

        [TestMethod]
        public void AddGuest_TicketNotInTour()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new List<string>() };

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber)).Returns(guest);
            _startTourFlow.SetTour(tour);

            // Act
            var addGuestResult = _startTourFlow.AddGuest(ticketNumber);

            // Assert
            Assert.IsFalse(addGuestResult.Success);
            Assert.AreEqual("ticketNotInTour", addGuestResult.MessageKey);
        }

        [TestMethod]
        public void SetTour_TourIsNull()
        {
            // Act
            var setTourResult = _startTourFlow.SetTour(null);

            // Assert
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourIsNull", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SetTour_TourIsFull()
        {
            // Arrange
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = Enumerable.Range(1, 13).Select(x => x.ToString()).ToList() };

            // Act
            var setTourResult = _startTourFlow.SetTour(tour);

            // Assert
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourFull", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SetTour_TourIsInPast()
        {
            // Arrange
            Tour tour = new Tour(DateTime.Now.AddHours(-1)) { Capacity = 13, Departed = false, Participants = new List<string>() };

            // Act
            var setTourResult = _startTourFlow.SetTour(tour);

            // Assert
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourInPast", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SetTour_TourHasDeparted()
        {
            // Arrange
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = true, Participants = new List<string>() };

            // Act
            var setTourResult = _startTourFlow.SetTour(tour);

            // Assert
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourDeparted", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SetEmployeeNumber_EmployeeNumberIsNullOrWhiteSpace()
        {
            // Act
            var setEmployeeNumberResult = _startTourFlow.SetEmployeeNumber("   ");

            // Assert
            Assert.IsFalse(setEmployeeNumberResult.Success);
            Assert.AreEqual("employeeNumberIsNull", setEmployeeNumberResult.MessageKey);
        }

        [TestMethod]
        public void SetEmployeeNumber_InvalidEmployeeNumber()
        {
            // Arrange
            string employeeNumber = "001";

            _employeeServiceMock.Setup(x => x.ValidateEmployeeNumber(employeeNumber)).Returns(false);

            // Act
            var setEmployeeNumberResult = _startTourFlow.SetEmployeeNumber(employeeNumber);

            // Assert
            Assert.IsFalse(setEmployeeNumberResult.Success);
            Assert.AreEqual("employeeNumberInvalid", setEmployeeNumberResult.MessageKey);
        }

        [TestMethod]
        public void Commit_NoScannedTickets()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new List<string> { guest.TicketNumber } };
            string employeeNumber = "001";

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber)).Returns(guest);
            _employeeServiceMock.Setup(x => x.ValidateEmployeeNumber(employeeNumber)).Returns(true);

            _startTourFlow.SetTour(tour);
            _startTourFlow.SetEmployeeNumber(employeeNumber);

            // Act
            var commitResult = _startTourFlow.Commit();

            // Assert
            Assert.IsFalse(commitResult.Success);
            Assert.AreEqual("noScannedTickets", commitResult.MessageKey);
        }

        [TestMethod]
        public void Commit_TourIsNull()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };
            string employeeNumber = "001";

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber)).Returns(guest);
            _employeeServiceMock.Setup(x => x.ValidateEmployeeNumber(employeeNumber)).Returns(true);

            _startTourFlow.SetEmployeeNumber(employeeNumber);
            _startTourFlow.AddGuest(ticketNumber);

            // Act
            var commitResult = _startTourFlow.Commit();

            // Assert
            Assert.IsFalse(commitResult.Success);
            Assert.AreEqual("tourIsNull", commitResult.MessageKey);
        }

        [TestMethod]
        public void Commit_HappyFlow()
        {
            // Arrange
            string ticketNumber = "13548424";
            Guest guest = new Guest() { TicketNumber = ticketNumber };
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new List<string> { guest.TicketNumber } };
            string employeeNumber = "001";

            _guestServiceMock.Setup(x => x.FindValidGuestById(ticketNumber)).Returns(guest);
            _employeeServiceMock.Setup(x => x.ValidateEmployeeNumber(employeeNumber)).Returns(true);

            _startTourFlow.SetTour(tour);
            _startTourFlow.SetEmployeeNumber(employeeNumber);
            _startTourFlow.AddGuest(ticketNumber);

            // Act
            var commitResult = _startTourFlow.Commit();

            // Assert
            Assert.IsTrue(commitResult.Success);
        }
    }
}
