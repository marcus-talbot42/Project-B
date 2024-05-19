namespace ProjectB.Models
{
    public class Guest(DateOnly validDate) : AbstractUser
    {
        public DateOnly ValidDate { get; set; } = validDate;
        public Tour? Tour { get; set; }
    }
}