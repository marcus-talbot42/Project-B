using ProjectB.Resources;
using ProjectB.Models;

using ProjectB.Repositories;
using ProjectB.Services;

namespace ProjectB.login;

public class GuestLoginStrategy: ILoginStrategy
{
    
    private static IService<Translation, string> _translationService = new TranslationService(new TranslationRepository());
    // ((TranslationService) _translationService).GetTranslationString("chooseOption")
    
    public Session Handle()
    {
        
        Console.WriteLine(((TranslationService) _translationService).GetTranslationString("enterTicketNumber"));
        
        string? ticketNumber = Console.ReadLine();
        
        // TODO: Check whether ticket is valid

        return new Session(ticketNumber!, UserRole.Guest);
    }
}