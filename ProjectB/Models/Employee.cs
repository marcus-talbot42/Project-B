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
}