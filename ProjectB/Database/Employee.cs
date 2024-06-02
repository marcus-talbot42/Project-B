using ProjectB.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectB.Models;

public class Employee : AbstractEntity
{
    public string EmployeeNumber { get; set; } = null!;
    public string Username { get; set; } = null!;
    public UserRole Role { get; set; }
    [MaxLength(72)] public string? Password { get; set; }
}