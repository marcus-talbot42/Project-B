using ProjectB.IO;
using ProjectB.Models;

namespace ProjectB.Repositories;

public class UserRepository: AbstractRepository<User, string>
{
    
    private static readonly Lazy<UserRepository> Lazy = new(() => new UserRepository());
    public static UserRepository Instance => Lazy.Value;

    private UserRepository()
    {
        LoadUsers();
    }

    public IEnumerable<User> FindAllByRole(UserRole role) => 
        from user in Repository.Values 
        where user.GetUserRole() == role 
        select user;

    private void LoadUsers()
    {
        JsonFileReader<User> fileReader = new JsonFileReader<User>();
        ICollection<User>? users = fileReader.ReadAllObjects(".//users.json");
        
        if (users == null)
        {
            return;
        }
        
        foreach (User user in users)
        {
            Save(user);
        }
    }

    public override void Persist()
    {
        JsonFileWriter<User> fileWriter = new JsonFileWriter<User>();
        fileWriter.WriteObjects(".//users.json", Repository.Values);
    }

    public override void Refresh()
    {
        LoadUsers();
    }
}