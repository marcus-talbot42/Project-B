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
        
        var options = Enum.GetValues(typeof(settings.Language))
            .Cast<settings.Language>()
            .Select((value, index) => new { index, value })
            .ToDictionary(pair => pair.index + 1, pair => pair.value);
            
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title(((TranslationService) _translationService).GetTranslationString("chooseOption"))
                .PageSize(10)
                .AddChoices(options.Keys)
                .UseConverter(choice => $"{choice}. {((TranslationService) _translationService).GetTranslationString("lang_name_" + options[choice].ToString().ToLower())}")
        );
        
        try
        {
            Settings.Language = options[option]; // Attempt to switch the language
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"An error occurred while switching languages: {ex.Message}");

            // Revert back to the original language
            Settings.Language = settings.Language.NL;
        }
        
    }
}