using ProjectB.Models;

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
}