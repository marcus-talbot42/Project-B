using ProjectB.Models;
using ProjectB.Services;

namespace ProjectB.Views.Reservation;

public class CreateReservationView(TourService tourService) : IView
{
    public void Output()
    {
        // Show all tours, with an index
        PrintAllTours();

        // Get user input

        // Get tour corresponding to user choice

        // Register user for tour
    }

    private void PrintAllTours() {
        IEnumerable<Tour> allTours = tourService.GetAllToursTodayAfterNow();
        for (int i = 0; i < allTours.Count(); i++)
        {
            Console.WriteLine(allTours.ElementAt(i));
        }
    }
}