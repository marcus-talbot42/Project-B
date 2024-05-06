using ProjectB.Models;

namespace ProjectBTest.Fixtures;

public class TourFixtures
{
    
    public static ICollection<Tour> GenerateCollection(int amount)
    {
        
        ICollection<Employee> guides = EmployeeFixtures.GenerateGuides(amount);
        
        ICollection<Tour> userList = new HashSet<Tour>();
        for (int i = 0; i < amount; i++)
        {
            userList.Add(new Tour(new TourCompositeKey(DateTime.Now.AddHours(2), guides.ElementAt(i).GetId()), 13, new List<Guest>()));
        }

        return userList;
    }
}