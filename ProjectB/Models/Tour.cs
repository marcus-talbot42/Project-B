using System.Collections.Immutable;

namespace ProjectB.Models;

public class Tour(DateTime start, int capacity = 13)
    : AbstractEntity
{
    public ICollection<Guest> Participants { get; set; } = new List<Guest>();
    public DateTime Start { get; set; } = start;
    public int Capacity { get; set; } = capacity;
    public Employee Employee { get; set; }
}