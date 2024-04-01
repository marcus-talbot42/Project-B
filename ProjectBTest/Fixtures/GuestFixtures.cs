using ProjectB.Models;

namespace ProjectBTest.Fixtures;

public class GuestFixtures
{
    public static ICollection<Guest> GenerateCollection(int amount)
    {
        ICollection<Guest> userList = new List<Guest>();
        for (int i = 0; i < amount; i++)
        {
            userList.Add(new Guest(Guid.NewGuid().ToString(), DateOnly.FromDateTime(DateTime.Today)));
        }

        return userList;
    }
}