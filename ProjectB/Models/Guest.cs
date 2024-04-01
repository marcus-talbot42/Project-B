using Newtonsoft.Json;

namespace ProjectB.Models;

public class Guest(string username, DateOnly validForDate) : AbstractUser(username, UserRole.Guest)
{
    [JsonProperty] private readonly DateOnly _validForDate = validForDate;

    public bool IsValid() => _validForDate.CompareTo(DateTime.Today) == 0;
    
    public override bool Equals(AbstractUser? other)
    {
        if (ReferenceEquals(other, this)) return true;
        if (ReferenceEquals(other, null)) return false;
        if (other.GetType() != GetType()) return false;
        return Username == other.GetId();
    }
}