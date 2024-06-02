using ProjectB.Models;
using ProjectB.Enums;

namespace ProjectBTest.Fixtures;

public class EmployeeFixtures
{
    public static ICollection<Employee> GenerateCollection(int amount)
    {
        ICollection<Employee> userList = new List<Employee>();
        Random random = new Random();
        int enumLength = Enum.GetNames(typeof(UserRole)).Length;
        for (int i = 0; i < amount; i++)
        {
            // Does not include guest.
            UserRole role = (UserRole)random.Next(1, enumLength);
            userList.Add(new Employee(Guid.NewGuid().ToString(), role, Guid.NewGuid().ToString()));
        }

        return userList;
    }

    public static ICollection<Employee> GenerateGuides(int amount)
    {
        ICollection<Employee> userList = new List<Employee>();
        Random random = new Random();
        for (int i = 0; i < amount; i++)
        {
            // Does not include guest.
            userList.Add(new Employee(Guid.NewGuid().ToString(), UserRole.Guide, Guid.NewGuid().ToString()));
        }

        return userList;
    }

}