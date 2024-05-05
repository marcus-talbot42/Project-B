using ProjectB.Models;
using ProjectB.Services;
using ProjectB.settings;

namespace ProjectB.Views.Reservation;

public class CreateReservationView(TourService tourService, GuestService guestService) : IView
{
    public void Output()
    {
        IEnumerable<Tour> tours = tourService.GetAllToursTodayAfterNow();
        // Show all tours, with an index
        PrintAllTours(tours);

        // Get user input
        int input = GetInput(tours.Count());

        // Get tour corresponding to user choice
        Tour chosenTour = tours.ElementAt(input);

        // Register user for tour
        tourService.RegisterUserForTour(chosenTour, guestService.GetGuest(Settings.CurrentSession!.Username)!);
    }

    private void PrintAllTours(IEnumerable<Tour> tours) {
        for (int i = 0; i < tours.Count(); i++)
        {
            Tour tour = tours.ElementAt(i);
            Console.WriteLine($"{i}. Starts at: {tour.GetTourTime().Hour:00}:{tour.GetTourTime().Minute:00}, Remaining capacity: {tour.GetCapacity() - tour.GetParticipants().Count}");
        }
    }

    private int GetInput(int max) {
        int option;
        while (!int.TryParse(Console.ReadLine(), out option) || (option < 1 || option > max))
        {
            Console.WriteLine($"Invalid input. Please enter a number between 1 and {max}.");
        }
        return option;
    }
}