using ProjectB.settings;
using ProjectB.Views.Debug;
using ProjectB.Views.Login;

using ProjectB.Models;
using ProjectB.Repositories;
using ProjectB.Services;
using Spectre.Console;

namespace ProjectB.Views.Main;

public class MainMenuView(EmployeeLoginView employeeLoginView, GuestLoginView guestLoginView, DebugView debugView) : AbstractView
{

    private static IService<Translation, string> _translationService = new TranslationService(new TranslationRepository());
    
    
    public override void Output()
    {
        AnsiConsole.Clear();
        
        // Display a status message for 5 seconds
        AnsiConsole.Status().Start(((TranslationService) _translationService).GetTranslationString("wait"), ctx =>
        {
            ctx.Spinner(Spinner.Known.Material);
            ctx.Status($"\n {((TranslationService) _translationService).GetTranslationString("loadingData")}");
            Thread.Sleep(2000);
        });
        
        var options = new Dictionary<int, string>
        {
            { 1, $"[blue]{((TranslationService) _translationService).GetTranslationString("loginGuest")}[/]"},
            { 2, $"[blue]{((TranslationService) _translationService).GetTranslationString("loginEmployee")}[/]" },
            { 3, $"[blue]{((TranslationService) _translationService).GetTranslationString("switchLanguage")}[/]" },
            { 0, $"[blue]{((TranslationService) _translationService).GetTranslationString("debug")}[/]" }
        };
        
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title(((TranslationService) _translationService).GetTranslationString("chooseOption"))
                .PageSize(10)
                .AddChoices(options.Keys)
                .UseConverter(choice => $"{choice}. {options[choice]}")
        );
        
        
        while (true)
        {
            switch (option)
            {
                case 1:
                    guestLoginView.Output();
                    break;
                case 2:
                    employeeLoginView.Output();
                    break;
                case 3:
                    SwitchLanguage();
                    break;
                case 0:
                    debugView.Output();
                    break;
            }
        }
        
        static void SwitchLanguage()
        {
            AnsiConsole.Clear(); // Clear the console screen
            Dictionary<int, Language> languageOptions = new Dictionary<int, Language>
            {
                { 1, Language.EN },
                { 2, Language.NL }
            };

            foreach (var option in languageOptions)
            {
                AnsiConsole.MarkupLine($"{option.Key}. {option.Value}");
            }

            int selectedOption;
            do
            {
                selectedOption = AnsiConsole.Ask<int>("Enter your choice:");
            }
            while (!languageOptions.ContainsKey(selectedOption));

            Settings.Language = languageOptions[selectedOption];
        }
    }
}