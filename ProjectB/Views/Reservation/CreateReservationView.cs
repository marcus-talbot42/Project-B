using ProjectB.Models;
using ProjectB.Services;
using System.Text;

namespace ProjectB.Views.Reservation;

public class CreateReservationView(TourService tourService, GuestService guestService, TranslationService translationService) : AbstractView
{
    public override void Output()
    {
        var tours = tourService.GetAllToursTodayAfterNow();

        // Show all tours, with an index
        PrintAllTours(tours);

        // Get user input
        int option = ReadUserChoice(1, tours.Count(), $"{((TranslationService)translationService).GetTranslationString("choseTour")}\n{PrintAllTours(tours)}");
        // AnsiConsole.MarkupLine($"[blue]{((TranslationService) _translationService).GetTranslationString("enterTicketNumber")}[/]");


        // Get tour corresponding to user choice
        Tour selectedTour = tours.ElementAt(option - 1);

        // Register user for tour
        tourService.RegisterGuestForTour(guestService.FindValidGuestById(Settings.Settings.CurrentSession!.Username), selectedTour);
    }

    private string PrintAllTours(IEnumerable<Tour> tours)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < tours.Count(); i++)
        {
            var tour = tours.ElementAt(i);
            builder.Append($"{i + 1}. {((TranslationService)translationService).GetTranslationString("time")} {tour.Start.Hour:00}:{tour.Start:00}\t\t{((TranslationService)translationService).GetTranslationString("spots")} {tourService.GetRemainingCapacity(tour)}\n");
        }

        return builder.ToString();
    }
}