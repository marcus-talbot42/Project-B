namespace ProjectB.Models;

public class Tour(DateTime start, int capacity = 13) : AbstractEntity
{
    public List<string> Participants { get; set; } = new List<string>();
    public DateTime Start { get; set; } = start;
    public int Capacity { get; set; } = capacity;
    public string? Employee { get; set; }
}