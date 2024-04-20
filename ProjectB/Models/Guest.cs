using Newtonsoft.Json;

namespace ProjectB.Models
{
    public class Guest(string username, [JsonProperty] DateOnly validForDate) : AbstractUser(username, UserRole.Guest)
    {
        [JsonIgnore] private Tour? _tour;
        [JsonProperty] private readonly string _username = username;

        public void SetTour(Tour? tour)
        {
            _tour = tour;
        }

        public Tour? GetTour()
        {
            return _tour;
        }

        public new string GetId() => _username;
        
        public bool IsValid() => validForDate.CompareTo(DateTime.Today) == 0;

        public override bool Equals(AbstractUser? other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;
            if (other.GetType() != GetType()) return false;
            return GetId() == other.GetId();
        }
    }
}
