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
    }
}
