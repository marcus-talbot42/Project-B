
using ProjectB.Models;
using ProjectB.Repositories;
using ProjectB.Services;
using Spectre.Console;

namespace ProjectB.Views.Reservation;

public class ReservationView(
    CreateReservationView createReservationView,
    EditReservationView editReservationView,
    DeleteReservationView deleteReservationView) : AbstractView
{
    private static IService<Translation, string> _translationService = new TranslationService(new TranslationRepository());
    // ((TranslationService) _translationService).GetTranslationString("chooseOption")
    // $"[blue]{((TranslationService) _translationService).GetTranslationString("createReservationView")}[/]"


    public override void Output()
    {
        while (true)
        {
            var options = new Dictionary<int, string>
            {
                { 1, $"[blue]{((TranslationService) _translationService).GetTranslationString("createReservationView")}[/]"},
                { 2, $"[blue]{((TranslationService) _translationService).GetTranslationString("editReservationView")}[/]" },
                { 3, $"[blue]{((TranslationService) _translationService).GetTranslationString("deleteReservationView")}[/]" },
                { 0, $"[blue]{((TranslationService) _translationService).GetTranslationString("return")}[/]" }
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
                        createReservationView.Output();
                        break;
                    case 2:
                        editReservationView.Output();
                        break;
                    case 3:
                        deleteReservationView.Output();
                        break;
                    case 4:
                        return;
                }
            }
        }
    }
}