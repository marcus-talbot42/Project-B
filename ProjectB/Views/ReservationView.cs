namespace ProjectB.Views;

public class ReservationView : IView
{
    private const string OptionText = """
                                      1. Sign up for a tour
                                      2. Delete your sign-up for a tour
                                      3. Return to the previous menu
                                      """;

    private const string MenuText = $"""
                                     Welcome to out museum!
                                     Please choose an option:
                                     {OptionText}
                                     """;

    private const string RetryText = $"""
                                     Sorry! That input was not recognised as a valid option. 
                                     Please pick from one of the options below:
                                     {OptionText}
                                     """;

    private static readonly Lazy<ReservationView> Lazy = new(() => new ReservationView());
    public static ReservationView Instance => Lazy.Value;

    public void Execute()
    {
        ShowMenu();
        int option = ReadInput();
        switch (option)
        {
            
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine(MenuText);
    }

    private int ReadInput()
    {
        int option;
        while (!int.TryParse(Console.ReadLine(), out option) || !IsValidChoice(option))
        {
            Console.WriteLine(RetryText);
        }

        return option;
    }

    private bool IsValidChoice(int choice) => choice is > 0 and < 4;
}