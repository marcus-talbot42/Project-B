using ProjectB.Views.Admin;
using ProjectB.Models;
using ProjectB.Repositories;
using ProjectB.Services;
using Spectre.Console;

namespace ProjectB.Views.Debug;

public class DebugView(CreateGuestView createGuestView, CreateEmployeeView createEmployeeView) : AbstractView
{
    private static IService<Translation, string> _translationService = new TranslationService(new TranslationRepository());


    public override void Output()
    {
        
        var options = new Dictionary<int, string>
        {
            { 1, $"[blue]{((TranslationService) _translationService).GetTranslationString("createGuest")}[/]"},
            { 2, $"[blue]{((TranslationService) _translationService).GetTranslationString("createEmployee")}[/]" },
            { 3, $"[blue]{((TranslationService) _translationService).GetTranslationString("exit")}[/]" },
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
                    createGuestView.Output();
                    break;
                case 2:
                    createEmployeeView.Output();
                    break;
                case 3:
                    break;
                
            }
        }
        
    }
}