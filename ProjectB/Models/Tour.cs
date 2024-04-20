using Newtonsoft.Json;

namespace ProjectB.Models;

public class Tour(
    [JsonProperty] int id,
    [JsonProperty] DateTime time,
    [JsonProperty] string location,
    [JsonProperty] int capacity,
    [JsonProperty] ICollection<Guest> participants)
    : IEntity<int>
{
    public int GetId()
    {
        return id;
    }

    public DateTime GetTime()
    {
        return time;
    }

    public string GetLocation()
    {
        return location;
    }

    public int GetCapacity()
    {
        return capacity;
    }

    public ICollection<Guest> GetParticipants()
    {
        return participants;
    }
}