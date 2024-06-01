using ProjectB.Services;
using ProjectB.Views.Admin;
using Spectre.Console;

namespace ProjectB.Views.Debug;

public class DebugView(CreateGuestView createGuestView, CreateEmployeeView createEmployeeView, ITranslationService translationService) : AbstractView
{

    public override void Output()
    {

        var options = new Dictionary<int, string>
        {
            { 1, $"[blue]{((TranslationService) translationService).GetTranslationString("createGuest")}[/]"},
            { 2, $"[blue]{((TranslationService) translationService).GetTranslationString("createEmployee")}[/]" },
            { 3, $"[blue]{((TranslationService) translationService).GetTranslationString("exit")}[/]" },
        };

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title(((TranslationService)translationService).GetTranslationString("chooseOption"))
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