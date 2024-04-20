using Newtonsoft.Json;

namespace ProjectB.Models;

/// <summary>
/// Class representing an Employee. Implements AbstractUser, and is ready to be saved and read from JSON-files.
/// </summary>
/// <param name="username">The username of the employee.</param>
/// <param name="role">The role of the employee. Should be either Guide or DepartmentHead.</param>
/// <param name="password">The password assigned to the user. Should never be empty.</param>
public class Employee(string username, UserRole role, string password) : AbstractUser(username, role)
{
    
    [JsonProperty] private readonly string _password = password;

    public bool IsPasswordCorrect(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, _password);
    }

    public override bool Equals(AbstractUser? other)
    {
        if (ReferenceEquals(other, this)) return true;
        if (ReferenceEquals(other, null)) return false;
        if (other.GetType() != GetType()) return false;
        return username == other.GetId();
    }
}