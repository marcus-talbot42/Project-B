using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectB.Models;
using ProjectB;

namespace ProjectB.Tests
{
    [TestClass]
    public class ReservationTests
    {
        [TestMethod]
        public void SignUpForTour_UserAlreadySignedUp_UserNotAddedToTour()
        {
            // Test scenario: Attempting to sign up a user who is already signed up for a tour
            // Arrange: Create a mock tour with a participant
            var mockTour = new Tour(DateTime.Today, "Test Location", 10)
            {
                Participants = new List<Guest>
                {
                    new Guest("testUser", DateOnly.FromDateTime(DateTime.Today), "testUser")
                }
            };
            Reservation.tours = new List<Tour> { mockTour };
            var input = new StringReader("1\n");

            // Act: Try to sign up the same user again
            Console.SetIn(input);
            Reservation.SignUpForTour();
        }

        [TestMethod]
        public void DeleteSignUpForTour_UserSignedUp_UserRemovedFromTour()
        {
            // Test scenario: Deleting the sign-up of a user from a tour
            // Arrange: Create a mock tour with a participant
            var mockTour = new Tour(DateTime.Today, "Test Location", 10)
            {
                Participants = new List<Guest>
                {
                    new Guest("testUser", DateOnly.FromDateTime(DateTime.Today), "testUser")
                }
            };
            Reservation.tours = new List<Tour> { mockTour };
            var input = new StringReader("testUser\n");

            // Act: Delete the sign-up of the user
            Console.SetIn(input);
            Reservation.DeleteSignUpForTour();
        }
    }
}
