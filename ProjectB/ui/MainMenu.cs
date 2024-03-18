using ProjectB.login;
using ProjectB.Resources;
using ProjectB.settings;

namespace ProjectB.ui;

public class MainMenu
{

    public void ShowMenu()
    {
        Console.WriteLine(Translations.translation("MAIN_MENU"));
    }

    public void HandleInput(char input)
    {
        Console.WriteLine("\n");
        switch (input)
        {
            case '1':
                Settings.CurrentSession = new GuestLoginStrategy().Handle();
                break;
            case '2':
                Settings.CurrentSession = new EmployeeLoginStrategy().Handle();
                break;
            case '3':
                Settings.Language = Settings.Language == Language.NL ? Language.EN : Language.NL;
                Console.WriteLine(Translations.translation("LANGUAGE_CHANGED"));
                break;
        }
    }
    
}