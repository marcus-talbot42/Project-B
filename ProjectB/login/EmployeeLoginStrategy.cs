using System.Security;
using ProjectB.Models;
using ProjectB.Repositories;
using ProjectB.Resources;
using ProjectB.Services;
using Spectre.Console;

namespace ProjectB.login;

public class EmployeeLoginStrategy: ILoginStrategy
{
    private static IService<Translation, string> _translationService = new TranslationService(new TranslationRepository());
    // ((TranslationService) _translationService).GetTranslationString("chooseOption")
    // $"[blue]{((TranslationService) _translationService).GetTranslationString("createReservationView")}[/]"
        



    public Session Handle()
    {
        string? username = AnsiConsole.Ask<string>(((TranslationService) _translationService).GetTranslationString("enterUsername"));
        AnsiConsole.MarkupLine($"[blue]{((TranslationService) _translationService).GetTranslationString("enterPassword")}[/]");
        SecureString? password = GetPassword();
        
        // TODO: Check password, and get UserRole from file.

        return new Session(username!, UserRole.Guest); // TODO: user proper UserRole.
    }
    
    private SecureString GetPassword()
    {
        var pwd = new SecureString();
        while (true)
        {
            ConsoleKeyInfo i = Console.ReadKey(true);
            if (i.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (i.Key == ConsoleKey.Backspace)
            {
                if (pwd.Length > 0)
                {
                    pwd.RemoveAt(pwd.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else if (i.KeyChar != '\u0000' ) // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
            {
                pwd.AppendChar(i.KeyChar);
                Console.Write("*");
            }
        }
        return pwd;
    }
}