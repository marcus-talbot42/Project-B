namespace ProjectB.Models
{
    public class Guest(DateOnly validDate) : AbstractUser
    {
        public DateOnly ValidDate { get; set; } = validDate;
        public Tour? Tour { get; set; }

        public bool IsGuestInTour => Tour != null;

        public bool IsValid() => ValidDate.CompareTo(DateOnly.FromDateTime(DateTime.Today)) == 0;

        public override bool Equals(AbstractUser? other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;
            if (other.GetType() != GetType()) return false;
            return GetId() == other.GetId();
        }
    }
}
