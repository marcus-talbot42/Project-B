namespace ProjectB.Models;

public abstract class AbstractUser : AbstractEntity
{
    public string Username { get; set; }
    public UserRole Role { get; set; }
}
