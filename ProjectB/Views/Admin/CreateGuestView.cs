using ProjectB.Models;
using ProjectB.Services;

namespace ProjectB.Views.Admin;

public class CreateGuestView(GuestService service) : AbstractView
{
    public override void Output()
    {
        Console.WriteLine("Geef het ticketnummer van de gast in:");
        string ticketNummer = Console.ReadLine()!;
        Console.WriteLine("Geef de geldigheidsdatum van het ticket in (yyyy-mm-dd): ");
        DateOnly validForDate = DateOnly.Parse(Console.ReadLine()!);
        
        service.Create(new Guest(ticketNummer, validForDate));
    }
}