using ProjectB.Views;

namespace ProjectB;

public class Reservation
{
    static void Main(string[] args)
    {
        MainMenuView.Instance.Execute();
    }

    //     LoadParticipantsFromJson(); // Load sign-up data from JSON file, if exists
    //
    //     // Get the current hour
    //     int currentHour = DateTime.Now.Hour;
    //
    //     // Create tours for each hour from the current hour to 8 PM
    //     for (int hour = currentHour; hour <= 20; hour++)
    //     {
    //         DateTime tourTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, 0, 0);
    //         Tour tour = new Tour(0, tourTime, "Your Tour Location", 13); // Initialize tours with default capacity of 13
    //         tours.Add(tour);
    //     }
    //
    //     // Register event handler to save sign-up data on application exit
    //     AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
    //
    //     bool exit = false;
    //     while (!exit)
    //     {
    //         Console.WriteLine("Welcome to our museum!");
    //         Console.WriteLine("Please choose an option:");
    //         Console.WriteLine("1. Sign up for a tour");
    //         Console.WriteLine("2. Delete your sign-up for a tour");
    //         Console.WriteLine("3. Exit");
    //
    //         int option;
    //         while (!int.TryParse(Console.ReadLine(), out option) || (option < 1 || option > 3))
    //         {
    //             Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
    //         }
    //
    //         switch (option)
    //         {
    //             case 1:
    //                 SignUpForTour();
    //                 break;
    //             case 2:
    //                 DeleteSignUpForTour();
    //                 break;
    //             case 3:
    //                 Console.WriteLine("Thank you for visiting our museum! Goodbye.");
    //                 exit = true;
    //                 break;
    //         }
    //     }
    // }
    //
    // static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    // {
    //     SaveParticipantsToJson(); // Save sign-up data on application exit
    // }
    //
    // public static void SignUpForTour()
    // {
    //     Console.WriteLine("Available Tours:");
    //     for (int i = 0; i < tours.Count; i++)
    //     {
    //         int spotsLeft = tours[i].GetCapacity() - tours[i].GetParticipants().Count;
    //         Console.WriteLine($"{i + 1}. Tour at {tours[i].GetTime().ToString("HH:mm")}, {spotsLeft} spots left");
    //     }
    //
    //     Console.WriteLine("0. Go back");
    //
    //     Console.Write("Enter the number of the tour you want to sign up for (or 0 to go back): ");
    //     int tourNumber;
    //     while (!int.TryParse(Console.ReadLine(), out tourNumber) || (tourNumber < 0 || tourNumber > tours.Count))
    //     {
    //         Console.WriteLine($"Invalid input. Please enter a number between 0 and {tours.Count}.");
    //     }
    //
    //     if (tourNumber == 0)
    //     {
    //         return; // Go back to main menu
    //     }
    //
    //     Tour selectedTour = tours[tourNumber - 1];
    //
    //     if (selectedTour.GetParticipants().Count >= selectedTour.GetCapacity())
    //     {
    //         Console.WriteLine(
    //             $"Sorry, the tour at {selectedTour.GetTime().ToString("HH:mm")} is already fully booked.");
    //         return;
    //     }
    //
    //     Console.Write("Enter your username: ");
    //     string? username = Console.ReadLine();
    //
    //     if (selectedTour.GetParticipants().Any(p => p.GetId() == username))
    //     {
    //         Console.WriteLine($"Username {username} is already signed up for a tour.");
    //
    //         Console.Write("Do you want to change your sign-up (Y/N)? ");
    //         string? response = Console.ReadLine()!.Trim().ToUpper();
    //         if (response == "Y")
    //         {
    //             var guestToRemove = selectedTour.GetParticipants().FirstOrDefault(p => p.GetId() == username);
    //             if (guestToRemove != null)
    //             {
    //                 selectedTour.GetParticipants().Remove(guestToRemove); // Remove existing sign-up
    //             }
    //         }
    //         else
    //         {
    //             return; // Go back to main menu
    //         }
    //     }
    //
    //     Guest guest = new Guest(username!, DateOnly.FromDateTime(DateTime.Today));
    //     selectedTour.GetParticipants().Add(guest); // Add new sign-up
    //
    //     SaveParticipantsToJson(); // Save immediately after sign-up
    //
    //     Console.WriteLine(
    //         $"You have successfully signed up for the tour at {selectedTour.GetTime().ToString("HH:mm")}.");
    // }
    //
    // public static void DeleteSignUpForTour()
    // {
    //     Console.Write("Enter your username to delete your sign-up: ");
    //     string? usernameToDelete = Console.ReadLine();
    //
    //     foreach (var tour in tours)
    //     {
    //         var guestToRemove = tour.GetParticipants().FirstOrDefault(p => p.GetId() == usernameToDelete);
    //         if (guestToRemove != null)
    //         {
    //             tour.GetParticipants().Remove(guestToRemove); // Remove sign-up
    //
    //             SaveParticipantsToJson(); // Save immediately after deletion
    //
    //             Console.WriteLine($"Your sign-up for the tour at {tour.GetTime().ToString("HH:mm")} has been deleted.");
    //             return;
    //         }
    //     }
    //
    //     Console.WriteLine($"No sign-up found for username {usernameToDelete}.");
    // }
    //
    //
    // static void LoadParticipantsFromJson()
    // {
    //     if (File.Exists(jsonFilePath))
    //     {
    //         string json = File.ReadAllText(jsonFilePath);
    //         tours = JsonConvert.DeserializeObject<List<Tour>>(json)!;
    //     }
    // }
    //
    //
    // static void SaveParticipantsToJson()
    // {
    //     var filewriter = new JsonFileWriter<Tour>();
    //
    //     filewriter.WriteObjects(jsonFilePath, tours);
    // }
}