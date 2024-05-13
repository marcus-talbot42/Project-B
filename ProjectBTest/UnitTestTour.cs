using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectB.Models;
using System;

namespace ProjectB.Tests
{
    [TestClass]
    public class TourTests
    {
        // Test to ensure that the Tour constructor initializes properties correctly
        // [TestMethod]
        // public void Tour_Constructor_InitializesPropertiesCorrectly()
        // {
        //     DateTime time = new DateTime(2024, 4, 5, 10, 0, 0);
        //     string location = "Test Location";
        //     int capacity = 20;
        //
        //     Tour tour = new Tour(time, location, capacity);
        //
        //     Assert.AreEqual(time, tour.Time);
        //     Assert.AreEqual(location, tour.Location);
        //     Assert.AreEqual(capacity, tour.Capacity);
        //     Assert.IsNotNull(tour.Participants);
        //     Assert.AreEqual(0, tour.Participants.Count);
        // }
        //
        // // Test to ensure that a participant can be added to the participants list
        // [TestMethod]
        // public void Tour_AddParticipant_ParticipantAddedToParticipantsList()
        // {
        //     Tour tour = new Tour(DateTime.Now, "Test Location", 10);
        //     Guest guest = new Guest("testUser", DateOnly.FromDateTime(DateTime.Today), "testUser");
        //
        //     tour.Participants.Add(guest);
        //
        //     Assert.AreEqual(1, tour.Participants.Count);
        //     Assert.IsTrue(tour.Participants.Contains(guest));
        // }
        //
        // // Test to ensure that a participant can be removed from the participants list
        // [TestMethod]
        // public void Tour_RemoveParticipant_ParticipantRemovedFromParticipantsList()
        // {
        //     Tour tour = new Tour(DateTime.Now, "Test Location", 10);
        //     Guest guest = new Guest("testUser", DateOnly.FromDateTime(DateTime.Today), "testUser");
        //     tour.Participants.Add(guest);
        //
        //     tour.Participants.Remove(guest);
        //
        //     Assert.AreEqual(0, tour.Participants.Count);
        //     Assert.IsFalse(tour.Participants.Contains(guest));
        // }
    }
}
