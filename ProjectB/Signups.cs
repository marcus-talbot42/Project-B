using ProjectB.IO;
using ProjectB.Models;

public class TourSignUp
{
    public Dictionary<DateTime, Tour> SignUps { get; set; }

    public TourSignUp(Dictionary<DateTime, Tour> signUps)
    {
        SignUps = signUps;
    }

    public List<string> GetParticipants(DateTime tourTime)
    {
        List<string> participants = new List<string>();

        if (SignUps.ContainsKey(tourTime))
        {
            var tour = SignUps[tourTime];
            foreach (var participant in tour.Participants)
            {
                participants.Add(participant.GetId());
            }
        }

        return participants;
    }
}

class Program
{
    static void Main(string[] args)
    {
        IFileReader<Tour> fileReader = new JsonFileReader<Tour>();
        string jsonFilePath = "signups.json";
        var signUps = fileReader.ReadAllObjects(jsonFilePath);

        if (signUps != null)
        {
            Dictionary<DateTime, Tour> signups = new Dictionary<DateTime, Tour>();
            foreach (var tour in signUps)
            {
                signups.Add(tour.Time, tour);
            }
            TourSignUp signUp = new TourSignUp(signups);

            Console.WriteLine("Toets de tijd van de rondleiding in (format: HH:MM):");

            if (DateTime.TryParse(Console.ReadLine(), out DateTime tourTime))
            {
                var participants = signUp.GetParticipants(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tourTime.Hour, tourTime.Minute, 0));

                if (participants.Count == 0)
                {
                    Console.WriteLine("Geen deelnemers gevonden voor deze rondleiding.");
                }
                else
                {
                    Console.WriteLine($"Lijst van deelnemers van de rondleiding om {tourTime}:");
                    foreach (var participant in participants)
                    {
                        Console.WriteLine(participant);
                    }
                }
            }
            else
            {
                Console.WriteLine("Foute invoer. Voer een geldige tijd in.");
            }
        }
        else
        {
            Console.WriteLine("Het is mislukt om het signups bestand uit te lezen.");
        }
    }
}