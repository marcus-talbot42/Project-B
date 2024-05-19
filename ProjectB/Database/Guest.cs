namespace ProjectB.Models
{
    public class Guest : AbstractUser
    {
        public DateOnly ValidDate { get; set; }
        public Tour? Tour { get; set; }
    }
}