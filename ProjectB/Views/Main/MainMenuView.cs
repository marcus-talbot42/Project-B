using ProjectB.settings;
using ProjectB.Views.Debug;
using ProjectB.Views.Login;

namespace ProjectB.Views.Main;

public class MainMenuView(EmployeeLoginView employeeLoginView, GuestLoginView guestLoginView, DebugView debugView) : AbstractView
{

    private const string MENU_OPTIONS = """
                                        1. Inloggen als met uw ticket.
                                        2. Inloggen als medewerker.
                                        3. Change the language to English.
                                        """;
    private const string MENU_TEXT = $"""
                                        Welkom! Kies een van de onderstaande opties om verder te gaan.
                                        {MENU_OPTIONS}
                                        """;
    
    public override void Output()
    {
        Console.WriteLine(MENU_TEXT);
        while (true)
        {
            int option = ReadUserChoice(0, 3, $"Kies een van de onderstaande opties:\n{MENU_OPTIONS}");
            switch (option)
            {
                case 1:
                    Console.Clear();
                    guestLoginView.Output();
                    break;
                case 2:
                    employeeLoginView.Output();
                    break;
                case 3:
                    Settings.Language = Settings.Language == Language.EN ? Language.NL : Language.EN;
                    break;
                case 0:
                    debugView.Output();
                    break;
            }
        }
    }
}