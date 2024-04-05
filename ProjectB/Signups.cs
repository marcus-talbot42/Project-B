using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectB.IO;

public class TourSignUp
{
    public Dictionary<DateTime, List<int>> SignUps { get; set; }

    public TourSignUp(Dictionary<DateTime, List<int>> signUps)
    {
        SignUps = signUps;
    }

    public void GetParticipants(DateTime tourTime)
    {
        if (SignUps.ContainsKey(tourTime))
        {
            var participants = SignUps[tourTime];
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
            Console.WriteLine($"Er is geen rondleiding ingepland om {tourTime}.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        IFileReader<Dictionary<DateTime, List<int>>> fileReader = new JsonFileReader<Dictionary<DateTime, List<int>>>();
        string jsonFilePath = "signups.json";
        var signUps = fileReader.ReadAllObjects(jsonFilePath);

        if (signUps != null)
        {
            var tourSignUp = new TourSignUp(signUps.First());

            Console.WriteLine("Toets de tijd van de rondleiding in (format: HH:MM):");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime tourTime))
            {
                tourSignUp.GetParticipants(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tourTime.Hour, tourTime.Minute, 0));
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