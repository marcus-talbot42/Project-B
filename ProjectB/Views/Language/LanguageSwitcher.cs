using ProjectB.Services;
using Spectre.Console;

namespace ProjectB.Views.Language;

public class LanguageSwitcher(ITranslationService translationService) : AbstractView
{
    public override void Output()
    {
        AnsiConsole.Clear(); // Clear the console screen

        var options = Enum.GetValues(typeof(Settings.Language))
            .Cast<Settings.Language>()
            .Select((value, index) => new { index, value })
            .ToDictionary(pair => pair.index + 1, pair => pair.value);

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title(translationService.GetTranslationString("chooseOption"))
                .PageSize(10)
                .AddChoices(options.Keys)
                .UseConverter(choice => $"{choice}. {translationService.GetTranslationString("lang_name_" + options[choice].ToString().ToLower())}")
        );

        try
        {
            Settings.Settings.Language = options[option]; // Attempt to switch the language
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"An error occurred while switching languages: {ex.Message}");

            // Revert back to the original language
            Settings.Settings.Language = Settings.Language.NL;
        }

    }
}