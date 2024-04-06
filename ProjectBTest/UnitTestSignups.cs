using ProjectB.Models;
using ProjectB;

namespace ProjectB.Tests
{
    [TestClass]
    public class TourSignUpTests
    {
        [TestMethod]
        public void GetParticipants_NoParticipants_ReturnsEmptyList()
        {
            var signUps = new Dictionary<DateTime, Tour>();
            var tourSignUp = new TourSignUp(signUps);
            var tourTime = new DateTime(2024, 4, 6, 10, 0, 0);

            var result = tourSignUp.GetParticipants(tourTime);

            CollectionAssert.AreEqual(new List<string>(), result);
        }

        [TestMethod]
        public void GetParticipants_WithParticipants_ReturnsCorrectList()
        {

        }

        [TestMethod]
        public void GetParticipants_TourTimeNotInSignUps_ReturnsEmptyList()
        {

        }
    }
}
