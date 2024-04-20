using ProjectB.Models;

namespace ProjectB.login;

public class Session(AbstractUser currentUser)
{
    public AbstractUser GetCurrentUser()
    {
        return currentUser;
    }
}