using System.Text;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.settings;
using ProjectB.Repositories;
using Spectre.Console;

namespace ProjectB.Views.Reservation;

public class CreateReservationView(TourService tourService, GuestService guestService) : AbstractView
{
    private static IService<Translation, string> _translationService = new TranslationService(new TranslationRepository());
    
    public override void Output()
    {
        var tours = tourService.GetAllToursTodayAfterNow();
        
        // Show all tours, with an index
        PrintAllTours(tours);

        // Get user input
        int option = ReadUserChoice(1, tours.Count(), $"{((TranslationService) _translationService).GetTranslationString("choseTour")}\n{PrintAllTours(tours)}");
        // AnsiConsole.MarkupLine($"[blue]{((TranslationService) _translationService).GetTranslationString("enterTicketNumber")}[/]");


        // Get tour corresponding to user choice
        Tour selectedTour = tours.ElementAt(option - 1);

        // Register user for tour
        tourService.RegisterGuestForTour(guestService.FindValidGuestById(Settings.CurrentSession.Username), selectedTour);
    }

    private string PrintAllTours(IEnumerable<Tour> tours)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < tours.Count(); i++)
        {
            var tour = tours.ElementAt(i);
            builder.Append($"{i + 1}. {((TranslationService) _translationService).GetTranslationString("time")} {tour.GetTourTime().Hour:00}:{tour.GetTourTime():00}\t\t{((TranslationService) _translationService).GetTranslationString("spots")} {tour.GetRemainingCapacity()}\n");
        }

        return builder.ToString();
    }
}