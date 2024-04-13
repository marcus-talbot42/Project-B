using System;
using System.Collections.Generic;

namespace ProjectB.Models
{
    public class Tour : IEntity<DateTime>
    {
        public DateTime Time { get; set; } // Time of the tour
        public string Location { get; set; } // Location of the tour
        public int Capacity { get; set; } // Maximum capacity of the tour
        public List<Guest> Participants { get; set; } // Collection of participants signed up for the tour

        public Tour(DateTime time, string location, int capacity)
        {
            Time = time;
            Location = location;
            Capacity = capacity;
            Participants = new List<Guest>(capacity); // Initialize participant collection
        }

        public DateTime GetId() => Time;
    }
}
