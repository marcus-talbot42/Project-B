using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class Reservation
{
    static List<Tour> tours = new List<Tour>(); // List to store available tours
    static string jsonFilePath = "signups.json"; // File path to store sign-up data in JSON format

    static void Main(string[] args)
    {
        LoadParticipantsFromJson(); // Load sign-up data from JSON file, if exists

        // Get the current hour
        int currentHour = DateTime.Now.Hour;

        // Create tours for each hour from the current hour to 8 PM
        for (int hour = currentHour; hour <= 20; hour++)
        {
            DateTime tourTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, 0, 0);
            Tour tour = new Tour(tourTime, "Your Tour Location", 13); // Initialize tours with default capacity of 13
            tours.Add(tour);
        }

        // Register event handler to save sign-up data on application exit
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Welcome to our museum!");
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Sign up for a tour");
            Console.WriteLine("2. Delete your sign-up for a tour");
            Console.WriteLine("3. Exit");

            int option;
            while (!int.TryParse(Console.ReadLine(), out option) || (option < 1 || option > 3))
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
            }

            switch (option)
            {
                case 1:
                    SignUpForTour();
                    break;
                case 2:
                    DeleteSignUpForTour();
                    break;
                case 3:
                    Console.WriteLine("Thank you for visiting our museum! Goodbye.");
                    exit = true;
                    break;
            }
        }
    }

    static void CurrentDomain_ProcessExit(object sender, EventArgs e)
    {
        SaveParticipantsToJson(); // Save sign-up data on application exit
    }

    static void SignUpForTour()
    {
        Console.WriteLine("Available Tours:");
        for (int i = 0; i < tours.Count; i++)
        {
            int spotsLeft = tours[i].Capacity - tours[i].Participants.Count;
            Console.WriteLine($"{i + 1}. Tour at {tours[i].Time.ToString("HH:mm")}, {spotsLeft} spots left");
        }
        Console.WriteLine("0. Go back");

        Console.Write("Enter the number of the tour you want to sign up for (or 0 to go back): ");
        int tourNumber;
        while (!int.TryParse(Console.ReadLine(), out tourNumber) || (tourNumber < 0 || tourNumber > tours.Count))
        {
            Console.WriteLine($"Invalid input. Please enter a number between 0 and {tours.Count}.");
        }

        if (tourNumber == 0)
        {
            return; // Go back to main menu
        }

        Tour selectedTour = tours[tourNumber - 1];

        if (selectedTour.Participants.Count >= selectedTour.Capacity)
        {
            Console.WriteLine($"Sorry, the tour at {selectedTour.Time.ToString("HH:mm")} is already fully booked.");
            return;
        }

        Console.Write("Enter your ticket number: ");
        int ticketNumber;
        while (!int.TryParse(Console.ReadLine(), out ticketNumber) || ticketNumber <= 0)
        {
            Console.WriteLine("Invalid input. Ticket number must be a positive integer.");
            Console.Write("Enter your ticket number: ");
        }

        if (selectedTour.Participants.Any(p => p.TicketNumber == ticketNumber))
        {
            Console.WriteLine($"Ticket number {ticketNumber} is already signed up for a tour.");

            Console.Write("Do you want to change your sign-up (Y/N)? ");
            string response = Console.ReadLine().Trim().ToUpper();
            if (response == "Y")
            {
                var participantToRemove = selectedTour.Participants.FirstOrDefault(p => p.TicketNumber == ticketNumber);
                selectedTour.Participants.Remove(participantToRemove); // Remove existing sign-up
            }
            else
            {
                return; // Go back to main menu
            }
        }

        Participant participant = new Participant(ticketNumber);
        selectedTour.Participants.Add(participant); // Add new sign-up

        SaveParticipantsToJson(); // Save immediately after sign-up

        Console.WriteLine($"You have successfully signed up for the tour at {selectedTour.Time.ToString("HH:mm")}.");
    }

    static void DeleteSignUpForTour()
    {
        Console.Write("Enter your ticket number to delete your sign-up: ");
        int ticketNumberToDelete;
        while (!int.TryParse(Console.ReadLine(), out ticketNumberToDelete) || ticketNumberToDelete <= 0)
        {
            Console.WriteLine("Invalid input. Ticket number must be a positive integer.");
        }

        foreach (var tour in tours)
        {
            var participantToRemove = tour.Participants.FirstOrDefault(p => p.TicketNumber == ticketNumberToDelete);
            if (participantToRemove != null)
            {
                tour.Participants.Remove(participantToRemove); // Remove sign-up

                SaveParticipantsToJson(); // Save immediately after deletion

                Console.WriteLine($"Your sign-up for the tour at {tour.Time.ToString("HH:mm")} has been deleted.");
                return;
            }
        }

        Console.WriteLine($"No sign-up found for ticket number {ticketNumberToDelete}.");
    }

    static void LoadParticipantsFromJson()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, List<int>>>(json);

            foreach (var kvp in dictionary)
            {
                DateTime tourTime = DateTime.Parse(kvp.Key);
                Tour tour = tours.FirstOrDefault(t => t.Time == tourTime);
                if (tour != null)
                {
                    foreach (var ticketNumber in kvp.Value)
                    {
                        Participant participant = new Participant(ticketNumber);
                        tour.Participants.Add(participant); // Add participant to existing tour
                    }
                }
            }
        }
    }

    static void SaveParticipantsToJson()
    {
        var dataToSerialize = new Dictionary<string, List<int>>();

        foreach (var tour in tours)
        {
            dataToSerialize.Add(tour.Time.ToString(), tour.Participants.Select(p => p.TicketNumber).ToList());
        }

        string json = JsonSerializer.Serialize(dataToSerialize);
        File.WriteAllText(jsonFilePath, json); // Write sign-up data to JSON file
    }
}
