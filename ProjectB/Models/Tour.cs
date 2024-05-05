using System;
using System.Collections.Generic;

namespace ProjectB.Models;

public class Tour(TourCompositeKey key, int capacity, ICollection<Guest> participants) : IEntity<TourCompositeKey>
{

    public DateTime GetTourTime() {
        return key.Time;
    }

    public Employee GetGuide() {
        return key.Guide;
    }

    public void SetGuide(Employee guide) {
        key.SetGuide(guide);
    }

    public ICollection<Guest> GetParticipants() {
        return participants;
    }

    public int GetCapacity() {
        return capacity;
    }

    public TourCompositeKey GetId()
    {
        return key;
    }
}