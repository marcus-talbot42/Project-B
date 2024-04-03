using ProjectB.Models;
class Tour
{
    public DateTime Time { get; set; } // Time of the tour
    public string Location { get; set; } // Location of the tour
    public int Capacity { get; set; } // Maximum capacity of the tour
    public ICollection<Guest> Participants { get; set; } // Collection of participants signed up for the tour

    public Tour(DateTime time, string location, int capacity)
    {
        Time = time;
        Location = location;
        Capacity = capacity;
        Participants = new List<Guest>(); // Initialize participant collection
    }
}
