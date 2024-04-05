using Newtonsoft.Json;

namespace ProjectB.Models
{
    public class Guest : AbstractUser
    {
        [JsonProperty]
        private readonly DateOnly _validForDate;

        public new string GetId() => Username;

        public Guest(string username, DateOnly validForDate, string usernameParam) : base(username, UserRole.Guest)
        {
            _validForDate = validForDate;
            this.Username = username;
        }

        public bool IsValid() => _validForDate.CompareTo(DateTime.Today) == 0;

        public override bool Equals(AbstractUser? other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;
            if (other.GetType() != GetType()) return false;
            return GetId() == other.GetId();
        }
    }
}
