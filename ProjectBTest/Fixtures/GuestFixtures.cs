using ProjectB.Models;
using System;
using System.Collections.Generic;

namespace ProjectBTest.Fixtures
{
    public class GuestFixtures
    {
        public static ICollection<Guest> GenerateCollection(int amount)
        {
            ICollection<Guest> userList = new List<Guest>();
            for (int i = 0; i < amount; i++)
            {
                // Generate a unique ticket number for each guest
                int ticketNumber = i + 1;
                
                userList.Add(new Guest(Guid.NewGuid().ToString(), DateOnly.FromDateTime(DateTime.Today), ticketNumber));
            }

            return userList;
        }
    }
}
