using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProjectB.IO;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Views.Reservation;

public class ReservationView(CreateReservationView createReservationView) : IView
{

    private const string OUTPUT = """
        Welcome to our museum!
        Please choose an option:
        1. Sign up for a tour
        2. Delete your sign-up for a tour
        3. Adjust Tour
        4. View participants for a tour
        5. Exit\n
        """;

    public void Output()
    {
        int option;
        Console.WriteLine(OUTPUT);
        while (!int.TryParse(Console.ReadLine(), out option) || (option < 1 || option > 5))
        {
            Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
        }

        switch(option) {
            case 1:
                createReservationView.Output();
                break;
        }
    }
}