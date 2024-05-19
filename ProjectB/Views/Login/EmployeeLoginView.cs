using ProjectB.Exceptions;
using ProjectB.login;
using ProjectB.Models;
using ProjectB.Services;

namespace ProjectB.Views.Login;

public class EmployeeLoginView(EmployeeService service) : AbstractView
{
    // LOGIN_TEXT
    private const string LOGIN_TEXT = "Please enter your username and password to log in:";

    // Implement GetUsername
    private string GetUsername()
    {
        Console.Write("Username: ");
        return Console.ReadLine();
    }

    // Implement GetPassword
    private string GetPassword()
    {
        Console.Write("Password: ");
        return Console.ReadLine();
    }

    public override void Output()
    {
        // Implement
        Console.WriteLine(LOGIN_TEXT);
        Employee? employee = null;
        do
        {
            string username = GetUsername()!;
            string password = GetPassword()!;
            try
            {
                employee = service.FindValidEmployeeByUsernameAndPassword(username, password);
                if (employee != null)
                {
                    Settings.Settings.CurrentSession = new Session(employee.Username, employee.Role);
                    Console.WriteLine("Login success...");
                }
                else
                {
                    Console.WriteLine("Incorrect password. Please try again.");
                }
            }
            catch (EntityNotFoundException)
            {
                Console.WriteLine("That username is not recognized. Please try again.");
            }
        } while (employee == null);
    }
}