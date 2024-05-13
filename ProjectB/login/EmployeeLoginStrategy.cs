using System.Security;
using ProjectB.Resources;
using ProjectB.Models;

namespace ProjectB.login;

public class EmployeeLoginStrategy: ILoginStrategy
{
    public Session Handle()
    {
        Console.WriteLine(Translations.translation("LOGIN_PROMPT_EMPLOYEE"));
        string? username = Console.ReadLine();
        Console.WriteLine(Translations.translation("LOGIN_PROMPT_EMPLOYEE_PASSWORD"));
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