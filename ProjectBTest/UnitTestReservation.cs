using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ProjectB.Models;
using ProjectB.Repositories;
using ProjectB;
using ProjectB.IO;
using static Reservation;

namespace ProjectB.Tests
{
    [TestClass]
    public class ReservationTests
    {
        private TourRepository? _repository;
        private Tour? _testTour;

        [TestInitialize]
        public void Initialize()
        {
            _repository = TourRepository.Instance;
            _testTour = new Tour(DateTime.Today, "Test Tour", 10);
            _repository.Save(_testTour);
        }

        [TestMethod]
        public void SignUpForTour()
        {
            SignUpForTour();
            Assert.AreEqual(1, _testTour!.Participants.Count);
        }

        [TestMethod]
        public void SignUpForTourTourFull()
        {
            _testTour!.Capacity = 0;
            SignUpForTour();
            Assert.AreEqual(0, _testTour!.Participants.Count);
        }

        [TestMethod]
        public void SignUpForTourAlreadySignedUp()
        {
            var testParticipant = new Guest("TestUser", DateOnly.FromDateTime(DateTime.Today), "TestUser");
            _testTour!.Participants.Add(testParticipant);
            SignUpForTour();
            Assert.AreEqual(1, _testTour!.Participants.Count);
        }

        [TestMethod]
        public void AdjustTourAddGuest()
        {
            var initialParticipantsCount = _testTour!.Participants.Count;
            ModifyTour();
            Assert.AreEqual(initialParticipantsCount + 1, _testTour.Participants.Count);
        }

        [TestMethod]
        public void AdjustTourDeleteGuest()
        {
            var testParticipant = new Guest("TestUser", DateOnly.FromDateTime(DateTime.Today), "TestUser");
            _testTour!.Participants.Add(testParticipant);
            var initialParticipantsCount = _testTour.Participants.Count;
            ModifyTour();
            Assert.AreEqual(initialParticipantsCount - 1, _testTour.Participants.Count);
        }
    }
}
