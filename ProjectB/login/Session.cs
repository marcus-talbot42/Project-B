using ProjectB.Enums;

namespace ProjectB.login;

public class Session
{
    public string Username { get; private set; }
    public UserRole Role { get; private set; }

    public Session(string username, UserRole role)
    {
        Username = username;
        Role = role;
    }
}