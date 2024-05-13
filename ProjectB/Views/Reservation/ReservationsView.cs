namespace ProjectB.Views.Reservation;

public class ReservationView(
    CreateReservationView createReservationView,
    EditReservationView editReservationView,
    DeleteReservationView deleteReservationView) : AbstractView
{
    private const string OPTIONS = """
                                   1. Inschrijven voor een tour.
                                   2. Uw inschrijving voor een tour wijzigen.
                                   3. Verwijder uw inschrijving voor een tour.
                                   4. Exit
                                   """;

    private const string OUTPUT = $"""
                                   Kies een van de onderstaande opties om verder te gaan:
                                   {OPTIONS}
                                   """;

    public override void Output()
    {
        while (true)
        {
            Console.WriteLine(OUTPUT);
            int option = ReadUserChoice(1, 4, OUTPUT);
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