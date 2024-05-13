using Spectre.Console;
using ProjectB.Services;
using ProjectB.settings;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Views.Language;

public class LanguageSwitcher() : AbstractView
{
    private static IService<Translation, string> _translationService = new TranslationService(new TranslationRepository());

    public override void Output()
    {
        AnsiConsole.Clear(); // Clear the console screen
            
        var options = new Dictionary<int, settings.Language>
        {
            { 1, settings.Language.EN },
            { 2, settings.Language.NL }
        };
            
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title(((TranslationService) _translationService).GetTranslationString("chooseOption"))
                .PageSize(10)
                .AddChoices(options.Keys)
                .UseConverter(choice => $"{choice}. {((TranslationService) _translationService).GetTranslationString("lang_name_" + options[choice].ToString().ToLower())}")
        );
            
        Settings.Language = options[option];
        
    }
}