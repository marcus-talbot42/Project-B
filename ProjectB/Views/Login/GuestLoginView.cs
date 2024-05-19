using ProjectB.Exceptions;
using ProjectB.login;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Views.Reservation;
using Spectre.Console;

namespace ProjectB.Views.Login;

public class GuestLoginView(GuestService service, ReservationView guestMenuView, TranslationService translationService) : AbstractView
{
    public override void Output()
    {
        AnsiConsole.MarkupLine($"[blue]{((TranslationService) translationService).GetTranslationString("enterTicketNumber")}[/]");

        Guest? guest = null;
        do
        {
            string ticketNumber = GetTicketNumber()!;
            try
            {
                guest = service.FindValidGuestById(ticketNumber);
                Settings.Settings.CurrentSession = new Session(guest.Username, UserRole.Guest);
                guestMenuView.Output();
            }
            catch (EntityNotFoundException exception)
            {
                AnsiConsole.Clear();
                // Console.WriteLine("Dat ticketnummer is niet herkend. Voor alstublieft nogmaals uw ticketnummer in:");
                AnsiConsole.MarkupLine($"[red]{((TranslationService) translationService).GetTranslationString("ticketNotFound")}[/]");
                
            }
        } while (guest == null);

    }

    private string? GetTicketNumber()
    {
        // return Console.ReadLine();
        return AnsiConsole.Ask<string>(((TranslationService) translationService).GetTranslationString("ticketNumber"));
    }
}