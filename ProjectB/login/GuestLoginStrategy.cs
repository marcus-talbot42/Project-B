using ProjectB.Resources;
using ProjectB.Models;

namespace ProjectB.login;

public class GuestLoginStrategy: ILoginStrategy
{
    
    
    
    public Session Handle()
    {
        Console.WriteLine(Translations.translation("LOGIN_PROMPT_GUEST"));
        string? ticketNumber = Console.ReadLine();
        
        // TODO: Check whether ticket is valid

        return new Session(ticketNumber!, UserRole.Guest);
    }
}