using ProjectB.Exceptions;
using ProjectB.login;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.settings;
using ProjectB.Views.Reservation;

namespace ProjectB.Views.Login;

public class GuestLoginView(GuestService service, ReservationView guestMenuView) : AbstractView
{

    private const string LOGIN_TEXT = "Geef uw ticketnummer in:";
    
    public override void Output()
    {
        Console.WriteLine(LOGIN_TEXT);
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
                Console.WriteLine("Dat ticketnummer is niet herkend. Voor alstublieft nogmaals uw ticketnummer in:");
            }
        } while (guest == null);

    }

    private string? GetTicketNumber()
    {
        return Console.ReadLine();
    }
}