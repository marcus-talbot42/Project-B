using ProjectB.Enums;

namespace ProjectB.Models
{
    public class Guest : AbstractEntity
    {
        public string TicketNumber { get; set; } = "";
        public UserRole Role { get; set; }
        public DateOnly ValidDate { get; set; }
    }
}