using ProjectB.Exceptions;
using ProjectB.login;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.settings;
using ProjectB.Views.Reservation;
using ProjectB.Repositories;
using Spectre.Console;

namespace ProjectB.Views.Login;

public class GuestLoginView(GuestService service, ReservationView guestMenuView) : AbstractView
{
    private static IService<Translation, string> _translationService = new TranslationService(new TranslationRepository());
    
    public override void Output()
    {
        AnsiConsole.MarkupLine($"[blue]{((TranslationService) _translationService).GetTranslationString("enterTicketNumber")}[/]");

        Guest? guest = null;
        do
        {
            string ticketNumber = GetTicketNumber()!;
            try
            {
                guest = service.FindValidGuestById(ticketNumber);
                Settings.CurrentSession = new Session(guest.GetId(), UserRole.Guest);
                guestMenuView.Output();
            }
            catch (EntityNotFoundException exception)
            {
                AnsiConsole.Clear();
                // Console.WriteLine("Dat ticketnummer is niet herkend. Voor alstublieft nogmaals uw ticketnummer in:");
                AnsiConsole.MarkupLine($"[red]{((TranslationService) _translationService).GetTranslationString("ticketNotFound")}[/]");
                
            }
        } while (guest == null);

    }

    private string? GetTicketNumber()
    {
        // return Console.ReadLine();
        return AnsiConsole.Ask<string>(((TranslationService) _translationService).GetTranslationString("ticketNumber"));
    }
}