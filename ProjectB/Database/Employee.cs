using System.ComponentModel.DataAnnotations;

namespace ProjectB.Models;

public class Employee : AbstractUser
{
    [MaxLength(72)] public string? Password { get; set; }
}