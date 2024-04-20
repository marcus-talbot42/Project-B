using ProjectB.login;
using ProjectB.Models;

namespace ProjectB.settings;

public class Settings
{
    public static Language Language = Language.NL;
    public static Session? CurrentSession;

    public static AbstractUser? GetCurrentUser() => CurrentSession?.GetCurrentUser();
    
}