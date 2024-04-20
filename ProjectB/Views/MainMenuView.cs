using ProjectB.login;
using ProjectB.Resources;
using ProjectB.settings;

namespace ProjectB.Views;

public class MainMenuView : IView
{

    private static readonly Lazy<MainMenuView> Lazy = new(() => new MainMenuView());
    public static MainMenuView Instance => Lazy.Value;
    
    

    private void ShowMenu()
    {
        Console.WriteLine(Translations.translation("MAIN_MENU"));
    }

    private void HandleInput(char input)
    {
        Console.WriteLine("\n");
        switch (input)
        {
            case '1':
                Settings.CurrentSession = GuestLoginStrategy.Instance.Handle();
                break;
            case '2':
                Settings.CurrentSession = EmployeeLoginStrategy.Instance.Handle();
                break;
            case '3':
                Settings.Language = Settings.Language == Language.NL ? Language.EN : Language.NL;
                Console.WriteLine(Translations.translation("LANGUAGE_CHANGED"));
                break;
        }
    }

    public void Execute()
    {
        ShowMenu();
        HandleInput(Console.ReadKey().KeyChar);
        
    }
}