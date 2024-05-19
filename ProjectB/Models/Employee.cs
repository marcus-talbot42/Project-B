using System.ComponentModel.DataAnnotations;

namespace ProjectB.Models;

public class Employee : AbstractUser
{
    
    [MaxLength(72)]
    public required string Password { get; set; }

    public bool IsPasswordCorrect(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, Password);
    }

    public override bool Equals(AbstractUser? other)
    {
        if (ReferenceEquals(other, this)) return true;
        if (ReferenceEquals(other, null)) return false;
        if (other.GetType() != GetType()) return false;
        return Id == other.GetId();
    }
}