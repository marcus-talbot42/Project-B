namespace ProjectB.Models
{
    public class Guest(DateOnly validDate) : AbstractUser
    {
        public DateOnly ValidDate { get; set; } = validDate;
        public Tour? Tour { get; set; }

        public bool IsGuestInTour => Tour != null;

        public bool IsValid() => ValidDate.CompareTo(DateOnly.FromDateTime(DateTime.Today)) == 0;
    }
}