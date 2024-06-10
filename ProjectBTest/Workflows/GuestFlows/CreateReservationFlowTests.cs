using Moq;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Workflows.GuestFlows;

namespace ProjectBTest.Workflows.GuestFlows
{
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
    }
}
