using ProjectB.Models;

namespace ProjectBTest.Fixtures;

public class UserFixtures
{
    public static ICollection<User> GenerateCollection(int amount)
    {
        ICollection<User> userList = new List<User>();
        Random random = new Random();
        int enumLength = Enum.GetNames(typeof(UserRole)).Length;
        for (int i = 0; i < amount; i++)
        {
            UserRole role = (UserRole)random.Next(enumLength);
            userList.Add(new User(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), role));
        }

        return userList;
    }
}