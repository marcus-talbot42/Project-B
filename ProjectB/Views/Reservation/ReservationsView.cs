
using ProjectB.Services;
using Spectre.Console;

namespace ProjectB.Views.Reservation;

public class ReservationView(
    CreateReservationView createReservationView,
    EditReservationView editReservationView,
    DeleteReservationView deleteReservationView,
    TranslationService translationService) : AbstractView
{

    public override void Output()
    {
        while (true)
        {
            var options = new Dictionary<int, string>
            {
                { 1, $"[blue]{translationService.GetTranslationString("createReservationView")}[/]"},
                { 2, $"[blue]{translationService.GetTranslationString("editReservationView")}[/]" },
                { 3, $"[blue]{translationService.GetTranslationString("deleteReservationView")}[/]" },
                { 0, $"[blue]{translationService.GetTranslationString("return")}[/]" }
            };
            
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title(translationService.GetTranslationString("chooseOption"))
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