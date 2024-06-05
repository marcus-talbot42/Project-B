using Moq;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Workflows.GuestFlows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ProjectBTest.Workflows.GuestFlows
{
    [TestClass]
    public class EditReservationFlowTests
    {
        private Mock<IDatabaseContext> _contextMock;
        private Mock<ITourService> _tourServiceMock;
        private EditReservationFlow _editReservationFlow;

        [TestInitialize]
        public void TestInitialize()
        {
            _contextMock = new Mock<IDatabaseContext>();
            _tourServiceMock = new Mock<ITourService>();

            _editReservationFlow = new EditReservationFlow(
                _contextMock.Object,
                _tourServiceMock.Object);
        }

        [TestMethod]
        public void HappyFlow()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };
            Tour currentTour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new() { guest.TicketNumber } };
            Tour newTour = new Tour(DateTime.Now.AddHours(2)) { Capacity = 13, Departed = false, Participants = new() };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns(currentTour);

            _tourServiceMock.Setup(x => x.EditRegistrationGuestForTour(guest, newTour))
                .Returns(true);

            // Act  
            var setGuestResult = _editReservationFlow.SetGuest(guest);
            var setTourResult = _editReservationFlow.SetTour(newTour);
            var commitResult = _editReservationFlow.Commit();

            // Assert  
            Assert.IsTrue(setGuestResult.Success);
            Assert.IsTrue(setTourResult.Success);
            Assert.IsTrue(commitResult.Success);
        }


        [TestMethod]
        public void GuestHasNoCurrentReservation()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns((Tour?)null);

            // Act  
            var setGuestResult = _editReservationFlow.SetGuest(guest);

            // Assert  
            Assert.IsFalse(setGuestResult.Success);
            Assert.AreEqual("noReservationFound", setGuestResult.MessageKey);
        }

        [TestMethod]
        public void CurrentTourHasDeparted()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };
            Tour currentTour = new Tour(DateTime.Now.AddHours(-1)) { Capacity = 13, Departed = true, Participants = new() { guest.TicketNumber } };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns(currentTour);

            // Act  
            var setGuestResult = _editReservationFlow.SetGuest(guest);

            // Assert  
            Assert.IsFalse(setGuestResult.Success);
            Assert.AreEqual("tourDeparted", setGuestResult.MessageKey);
        }

        [TestMethod]
        public void SelectedTourIsFull()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };
            Tour currentTour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new() { guest.TicketNumber } };
            Tour newTour = new Tour(DateTime.Now.AddHours(2))
            {
                Capacity = 13,
                Departed = false,
                Participants = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13" }
            };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns(currentTour);

            // Act  
            var setGuestResult = _editReservationFlow.SetGuest(guest);
            var setTourResult = _editReservationFlow.SetTour(newTour);

            // Assert  
            Assert.IsTrue(setGuestResult.Success);
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourFull", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SelectedTourIsInPast()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };
            Tour currentTour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new() { guest.TicketNumber } };
            Tour newTour = new Tour(DateTime.Now.AddHours(-1)) { Capacity = 13, Departed = false, Participants = new() };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns(currentTour);

            // Act  
            var setGuestResult = _editReservationFlow.SetGuest(guest);
            var setTourResult = _editReservationFlow.SetTour(newTour);

            // Assert  
            Assert.IsTrue(setGuestResult.Success);
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourInPast", setTourResult.MessageKey);
        }

        [TestMethod]
        public void SelectedTourHasDeparted()
        {
            // Arrange  
            Guest guest = new Guest() { TicketNumber = "13548424" };
            Tour currentTour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new() { guest.TicketNumber } };
            Tour newTour = new Tour(DateTime.Now.AddHours(2)) { Capacity = 13, Departed = true, Participants = new() };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns(currentTour);

            // Act  
            var setGuestResult = _editReservationFlow.SetGuest(guest);
            var setTourResult = _editReservationFlow.SetTour(newTour);

            // Assert  
            Assert.IsTrue(setGuestResult.Success);
            Assert.IsFalse(setTourResult.Success);
            Assert.AreEqual("tourDeparted", setTourResult.MessageKey);
        }
    }
}
