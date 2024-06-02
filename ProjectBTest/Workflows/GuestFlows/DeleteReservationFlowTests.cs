using Moq;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Workflows.GuestFlows;

namespace ProjectBTest.Workflows
{
    public class DeleteReservationFlowTests
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
            Tour tour = new Tour(DateTime.Now.AddHours(1)) { Capacity = 13, Departed = false, Participants = new() { guest.TicketNumber } };

            // Set up mocks for dependencies  
            _tourServiceMock.Setup(x => x.DeleteReservationGuest(guest))
                .Returns(true);

            _tourServiceMock.Setup(x => x.GetTourForGuest(guest))
                .Returns(tour);

            // Act  
            var setGuest = _createReservationFlow.SetGuest(guest);
            var commit = _createReservationFlow.Commit();

            // Assert  
            Assert.IsTrue(setGuest.Success);
            Assert.IsTrue(commit.Success);
        }
    }
}
