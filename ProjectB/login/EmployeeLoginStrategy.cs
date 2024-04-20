using System.Text;
using ProjectB.Models;
using ProjectB.Resources;
using ProjectB.Services;

namespace ProjectB.login;

public class EmployeeLoginStrategy: ILoginStrategy
{

    private static readonly Lazy<EmployeeLoginStrategy> Lazy = new (() => new EmployeeLoginStrategy());
    public static EmployeeLoginStrategy Instance => Lazy.Value;
    
    private readonly EmployeeService _service = EmployeeService.Instance;

    private EmployeeLoginStrategy()
    {
    }
    
    public Session Handle()
    {
        Employee? employee;
        Console.Clear();
        do
        {
            Console.WriteLine(Translations.translation("LOGIN_PROMPT_EMPLOYEE"));
            string? username = Console.ReadLine();
            Console.WriteLine(Translations.translation("LOGIN_PROMPT_EMPLOYEE_PASSWORD"));
            string password = GetPassword();

            employee = _service.Authenticate(username!, password);
            
        } while (employee == null);

        return new Session(employee);
    }
    
    private string GetPassword()
    {
        var pwd = new StringBuilder();
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
                    pwd.Remove(pwd.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }
            else if (i.KeyChar != '\u0000' ) // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
            {
                pwd.Append(i.KeyChar);
                Console.Write("*");
            }
        }
        return pwd.ToString();
    }
}