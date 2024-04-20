using ProjectB.Models;
using ProjectB.Resources;
using ProjectB.Services;

namespace ProjectB.login;

public class GuestLoginStrategy: ILoginStrategy
{

    private static readonly Lazy<GuestLoginStrategy> Lazy = new(() => new GuestLoginStrategy());
    public static GuestLoginStrategy Instance => Lazy.Value;
    
    private readonly GuestService _service = GuestService.Instance;

    private GuestLoginStrategy()
    {
    }

    public Session Handle()
    {
        Guest? guest;
        do
        {
            ShowMenu();
            string? ticketNumber = Console.ReadLine();
            guest = _service.FindValidGuestById(ticketNumber!);
        } while (guest == null);
        
        return new Session(guest);
    }

    private void ShowMenu()
    {
        Console.WriteLine(Translations.translation("LOGIN_PROMPT_GUEST"));
    }
}