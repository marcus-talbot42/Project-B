using System.Collections.Immutable;

namespace ProjectB.Models;

public class Tour(DateTime start, int capacity = 13)
    : AbstractEntity
{
    public ICollection<Guest> Participants { get; set; } = new List<Guest>();
    public DateTime Start { get; set; } = start;
    public int Capacity { get; set; } = capacity;
    public Employee Employee { get; set; }

    public DateTime GetTourTime()
    {
        return Start;
    }

    public int GetRemainingCapacity()
    {
        return Capacity - Participants.Count;
    }

    public Employee GetGuide()
    {
        return Employee;
    }

    public void SetGuide(Employee guide)
    {
        Employee = guide;
    }

    public ICollection<Guest> GetParticipants()
    {
        return Participants.ToImmutableList();
    }

    public int GetCapacity()
    {
        return Capacity;
    }
}