using System.Text;
using ProjectB.Models;
using ProjectB.Services;

namespace ProjectB.Views;

public class TourSignUpView : IView
{
    private static readonly Lazy<TourSignUpView> Lazy = new(() => new TourSignUpView());
    public static TourSignUpView Instance => Lazy.Value;

    private readonly Lazy<TourService> _service = new(() => TourService.Instance);

    public void Execute()
    {
        int option = 0;
        IEnumerable<Tour> availableOptions;
        do
        {
            availableOptions = _service.Value.FindAllRemainingToursToday();
            ShowAvailableTours(availableOptions);
        } while (!int.TryParse(Console.ReadLine(), out option) || !IsValidOption(option, availableOptions));

        switch (option)
        {
            case 0:
                return;
            default:
                Tour tour = availableOptions.ElementAt(option);
                _service.Value.AddReservation(tour);
                break;
        }
    }

    private void ShowAvailableTours(IEnumerable<Tour> tours)
    {
        StringBuilder builder = new("Available Tours:\n");
        var enumerable = tours as Tour[] ?? tours.ToArray();
        for (int i = 0; i < enumerable.Count(); i++)
        {
            Tour currentTour = enumerable[i];
            builder.Append(
                $"{i + 1}. Tour at {currentTour.GetTime().ToString("HH:mm")}, {currentTour.GetCapacity() - currentTour.GetParticipants().Count} spots left.\n");
        }

        Console.WriteLine(builder.ToString());
    }

    private bool IsValidOption(int option, IEnumerable<Tour> toursAtTimeOfSelection)
    {
        return option >= 0
               && option <= toursAtTimeOfSelection.Count() + 1
               && _service.Value.FindAllRemainingToursToday().Contains(toursAtTimeOfSelection.ElementAt(option - 1));
    }
}