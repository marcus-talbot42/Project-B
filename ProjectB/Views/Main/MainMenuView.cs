using ProjectB.Settings;
using ProjectB.Views.Debug;
using ProjectB.Views.Login;
using ProjectB.Views.Language;

using ProjectB.Services;
using Spectre.Console;

namespace ProjectB.Views.Main;

public class MainMenuView(EmployeeLoginView employeeLoginView, GuestLoginView guestLoginView, LanguageSwitcher languageSwitcher, DebugView debugView, TranslationService translationService) : AbstractView
{
    
    public override void Output()
    {
        while (true)
        {
            AnsiConsole.Clear();
            
            // Display a status message for 5 seconds
            AnsiConsole.Status().Start(translationService.GetTranslationString("wait"), ctx =>
            {
                ctx.Spinner(Spinner.Known.Material);
                ctx.Status($"\n {translationService.GetTranslationString("loadingData")}");
                Thread.Sleep(2000);
            });
            
            var options = new Dictionary<int, string>
            {
                { 1, $"[blue]{translationService.GetTranslationString("loginGuest")}[/]"},
                { 2, $"[blue]{translationService.GetTranslationString("loginEmployee")}[/]" },
                { 3, $"[blue]{translationService.GetTranslationString("switchLanguage")}[/]" },
                { 0, $"[blue]{translationService.GetTranslationString("debug")}[/]" }
            };
            
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title(translationService.GetTranslationString("chooseOption"))
                    .PageSize(10)
                    .AddChoices(options.Keys)
                    .UseConverter(choice => $"{choice}. {options[choice]}")
            );
            

            switch (option)
            {
                case 1:
                    guestLoginView.Output();
                    break;
                case 2:
                    employeeLoginView.Output();
                    break;
                case 3:
                    languageSwitcher.Output();
                    break;
                case 0:
                    debugView.Output();
                    break;
            }
        }
    }
}