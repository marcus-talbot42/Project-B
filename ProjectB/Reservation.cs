using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProjectB.IO;
using ProjectB.Models;
using ProjectB.Repositories;

public class Reservation
{
    static TourRepository repository = TourRepository.Instance;
    static string jsonFilePath = "signups.json";

    static void Main(string[] args)
    {

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Welcome to our museum!");
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Sign up for a tour");
            Console.WriteLine("2. Delete your sign-up for a tour");
            Console.WriteLine("3. Adjust Tour");
            Console.WriteLine("4. View participants for a tour");
            Console.WriteLine("5. Exit");
            Console.WriteLine();

            int option;
            while (!int.TryParse(Console.ReadLine(), out option) || (option < 1 || option > 5))
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
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
                    ModifyTour();
                    break;
                case 4:
                    ViewParticipantsForTour();
                    break;
                case 5:
                    Console.WriteLine("Thank you for visiting our museum! Goodbye.");
                    exit = true;
                    break;
            }
        }
    }

    public static void SignUpForTour()
    {
        Console.WriteLine("Available Tours:");
        ICollection<Tour> tours = (ICollection<Tour>) repository.FindAll();
        for (int i = 0; i < tours.Count(); i++)
        {
            Tour tour = tours.ElementAt(i);
            int spotsLeft = tour.Capacity - tour.Participants.Count();
            Console.WriteLine($"{i + 1}. Tour at {tour.Time.ToString("HH:mm")}, {spotsLeft} spots left");
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

        Tour selectedTour = tours.ElementAt(tourNumber - 1);

        if (selectedTour.Participants.Count >= selectedTour.Capacity)
        {
            Console.WriteLine($"Sorry, the tour at {selectedTour.Time.ToString("HH:mm")} is already fully booked.");
            return;
        }

        Console.Write("Enter your username: ");
        string? username = Console.ReadLine();

        if (selectedTour.Participants.Any(p => p.GetId() == username))
        {
            Console.WriteLine($"Username {username} is already signed up for a tour.");

            Console.Write("Do you want to change your sign-up (Y/N)? ");
            string? response = Console.ReadLine()!.Trim().ToUpper();
            if (response == "Y")
            {
                var guestToRemove = selectedTour.Participants.FirstOrDefault(p => p.GetId() == username);
                if (guestToRemove != null)
                {
                    selectedTour.Participants.Remove(guestToRemove); // Remove existing sign-up
                    repository.Persist();
                }
            }
            else
            {
                return; // Go back to main menu
            }
        }

        Guest guest = new Guest(username!, DateOnly.FromDateTime(DateTime.Today), username!);
        selectedTour.Participants.Add(guest); // Add new sign-up
        repository.Persist();

        Console.WriteLine($"You have successfully signed up for the tour at {selectedTour.Time.ToString("HH:mm")}.");
    }

    public static void DeleteSignUpForTour()
    {
        Console.Write("Enter your username to delete your sign-up: ");
        string? usernameToDelete = Console.ReadLine();

        foreach (var tour in repository.FindAll())
        {
            var guestToRemove = tour.Participants.FirstOrDefault(p => p.GetId() == usernameToDelete);
            if (guestToRemove != null)
            {
                tour.Participants.Remove(guestToRemove); // Remove sign-up
                repository.Persist();
                Console.WriteLine($"Your sign-up for the tour at {tour.Time.ToString("HH:mm")} has been deleted.");
                return;
            }
        }

        Console.WriteLine($"No sign-up found for username {usernameToDelete}.");
    }

    public static void ModifyTour()
    {
        Console.WriteLine("Choose a tour to adjust:");

        var tours = repository.FindAll();
        for (int i = 0; i < repository.Count(); i++)
        {
            Console.WriteLine($"{i + 1}. Tour at {tours.ElementAt(i).Time:HH:mm}");
        }

        Console.Write("Enter the number of the tour you want to adjust: ");
        int tourIndex;
        while (!int.TryParse(Console.ReadLine(), out tourIndex) || (tourIndex < 1 || tourIndex > repository.Count()))
        {
            Console.WriteLine($"Invalid input. Please enter a number between 1 and {repository.Count()}.");
        }

        Tour selectedTour = tours.ElementAt(tourIndex - 1);

        Console.WriteLine($"Participants for the tour at {selectedTour.Time.ToString("HH:mm")}:");
        foreach (var participant in selectedTour.Participants)
        {
            Console.WriteLine(participant.GetId());
        }

        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Add Guest");
        Console.WriteLine("2. Delete Guest");
        Console.WriteLine("0. Go back");

        int option;
        while (!int.TryParse(Console.ReadLine(), out option) || (option < 0 || option > 2))
        {
            Console.WriteLine("Invalid input. Please enter 0, 1, or 2.");
        }

        switch (option)
        {
            case 1:
                Console.Write("Enter the username of the guest to add: ");
                string? usernameToAdd = Console.ReadLine();

                if (selectedTour.Participants.Count >= selectedTour.Capacity)
                {
                    Console.WriteLine($"Sorry, the tour at {selectedTour.Time.ToString("HH:mm")} is already fully booked.");
                    return;
                }

                Guest newGuest = new Guest(usernameToAdd!, DateOnly.FromDateTime(DateTime.Today), usernameToAdd!);
                selectedTour.Participants.Add(newGuest);
                repository.Persist();

                Console.WriteLine($"Guest {usernameToAdd} added to the tour at {selectedTour.Time.ToString("HH:mm")}.");
                break;
            case 2:
                Console.Write("Enter the username of the guest to delete: ");
                string? usernameToDelete = Console.ReadLine();

                var guestToRemove = selectedTour.Participants.FirstOrDefault(p => p.GetId() == usernameToDelete);
                if (guestToRemove != null)
                {
                    Console.WriteLine($"Are you sure you want to delete guest {usernameToDelete}? (Y/N)");
                    string? confirmation = Console.ReadLine()?.Trim().ToUpper();
                    if (confirmation == "Y")
                    {
                        selectedTour.Participants.Remove(guestToRemove);
                        Console.WriteLine($"Guest {usernameToDelete} deleted from the tour at {selectedTour.Time.ToString("HH:mm")}.");
                    }
                    else
                    {
                        Console.WriteLine("Deletion cancelled.");
                    }
                }
                else
                {
                    Console.WriteLine($"Guest {usernameToDelete} not found in the tour at {selectedTour.Time.ToString("HH:mm")}.");
                }
                break;
            case 0:
                Console.WriteLine("Going back to main menu.");
                break;
        }
    }

    public static void ViewParticipantsForTour()
    {
        Console.WriteLine("Enter the time of the tour (format: HH:MM):");

        if (DateTime.TryParse(Console.ReadLine(), out DateTime tourTime))
        {
            var participants = repository.GetParticipants(tourTime);

            if (participants.Count == 0)
            {
                Console.WriteLine("No participants found for this tour.");
            }
            else
            {
                Console.WriteLine($"List of participants for the tour at {tourTime}:");
                foreach (var participant in participants)
                {
                    Console.WriteLine(participant);
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid time.");
        }
    }
}