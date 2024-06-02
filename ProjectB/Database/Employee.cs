using System.ComponentModel.DataAnnotations;
using ProjectB.Enums;

namespace ProjectB.Models;

public class Employee : AbstractEntity
{
    public string Username { get; set; }
    public UserRole Role { get; set; }
    [MaxLength(72)] public string? Password { get; set; }
}