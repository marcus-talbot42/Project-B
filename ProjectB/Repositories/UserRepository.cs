using ProjectB.Models;

namespace ProjectB.Repositories;

public class UserRepository: AbstractRepository<AbstractUser, string>
{
    
    private static readonly Lazy<UserRepository> Lazy = new(() => new UserRepository());
    public static UserRepository Instance => Lazy.Value;

    private UserRepository()
    {
        Refresh();
    }

    public IEnumerable<AbstractUser> FindAllByRole(UserRole role) => 
        from user in Repository.Values 
        where user.GetUserRole() == role 
        select user;
}