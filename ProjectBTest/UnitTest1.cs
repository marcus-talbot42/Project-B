namespace ProjectBTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestAdditionCalculator()
    {
    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

[TestFixture]
public class ReservationTests
{
    private Reservation reservation;

    [SetUp]
    public void Setup()
    {
        reservation = new Reservation();
    }

    [Test]
    public void SignUpForTour_ValidTicketNumberAndTourNumber_SuccessfullySignedUp()
    {
        // Arrange
        int tourNumber = 1;
        int ticketNumber = 12345;

        // Act
        reservation.SignUpForTour(tourNumber, ticketNumber);

        // Assert
        Assert.AreEqual(1, reservation.GetParticipantsCount());
    }

    [Test]
    public void SignUpForTour_InvalidTicketNumber_ReturnsErrorMessage()
    {
        // Arrange
        int tourNumber = 1;
        int ticketNumber = -12345;

        // Act
        string errorMessage = reservation.SignUpForTour(tourNumber, ticketNumber);

        // Assert
        Assert.AreEqual("Invalid input. Ticket number must be a positive integer.", errorMessage);
    }

    [Test]
    public void SignUpForTour_TourFullyBooked_ReturnsErrorMessage()
    {
        // Arrange
        int tourNumber = 1;
        int ticketNumber = 12345;

        // Add participants to fill up the tour
        for (int i = 0; i < 13; i++)
        {
            reservation.SignUpForTour(tourNumber, i + 1);
        }

        // Act
        string errorMessage = reservation.SignUpForTour(tourNumber, ticketNumber);

        // Assert
        Assert.AreEqual($"Sorry, the tour at {DateTime.Now.AddHours(tourNumber).ToString("HH:mm")} is already fully booked.", errorMessage);
    }

    [Test]
    public void DeleteSignUpForTour_ExistingTicketNumber_SuccessfullyDeleted()
    {
        // Arrange
        int tourNumber = 1;
        int ticketNumber = 12345;

        // Sign up for a tour
        reservation.SignUpForTour(tourNumber, ticketNumber);

        // Act
        reservation.DeleteSignUpForTour(ticketNumber);

        // Assert
        Assert.AreEqual(0, reservation.GetParticipantsCount());
    }

    [Test]
    public void DeleteSignUpForTour_NonExistingTicketNumber_ReturnsErrorMessage()
    {
        // Arrange
        int tourNumber = 1;
        int ticketNumber = 12345;

        // Sign up for a tour
        reservation.SignUpForTour(tourNumber, ticketNumber);

        // Act
        string errorMessage = reservation.DeleteSignUpForTour(54321);

        // Assert
        Assert.AreEqual($"No sign-up found for ticket number 54321.", errorMessage);
    }

    [Test]
    public void LoadParticipantsFromJson_JsonFileExists_ParticipantsLoadedSuccessfully()
    {
        // Arrange
        string jsonFilePath = "signups.json";
        File.WriteAllText(jsonFilePath, "{\"2022-01-01T09:00:00\":[{\"TicketNumber\":12345,\"TourTime\":\"2022-01-01T09:00:00\"}]}");

        // Act
        reservation.LoadParticipantsFromJson();

        // Assert
        Assert.AreEqual(1, reservation.GetParticipantsCount());

        // Clean up
        File.Delete(jsonFilePath);
    }

    [Test]
    public void SaveParticipantsToJson_ParticipantsExist_ParticipantsSavedToJsonFile()
    {
        // Arrange
        string jsonFilePath = "signups.json";
        reservation.SignUpForTour(1, 12345);

        // Act
        reservation.SaveParticipantsToJson();

        // Assert
        Assert.IsTrue(File.Exists(jsonFilePath));

        // Clean up
        File.Delete(jsonFilePath);
    }
}