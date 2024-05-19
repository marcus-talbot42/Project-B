namespace ProjectB.Models;

public abstract class AbstractUser : AbstractEntity, IEquatable<AbstractUser>
{

    public string Username { get; set; }
    public UserRole Role { get; set; }
    
    public UserRole GetUserRole()
    {
        return Role;
    }

    public string GetUsername()
    {
        return Username;
    }

    public abstract bool Equals(AbstractUser? other);
    
    public override bool Equals(object? obj)
    {
        return Equals(obj as AbstractUser);
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Username, (int)Role);
    }
}
