using ProjectB.Enums;

namespace ProjectB.Models
{
    public class Guest : AbstractEntity
    {
        public string TicketNumber { get; set; } = null!;
        public UserRole Role { get; set; }
        public DateOnly ValidDate { get; set; }
        public bool Expires { get; set; } = false;
    }
}