using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjectB.Models;

public class Tour : IEntity<TourCompositeKey>
{

    [JsonProperty] private readonly ICollection<Guest> _participants;
    [JsonProperty] private readonly int _capacity;
    [JsonProperty] private readonly TourCompositeKey _key;

    public Tour(TourCompositeKey key, int capacity, ICollection<Guest> participants)
    {
        _key = key;
        _capacity = capacity;
        _participants = participants;
    }

    public DateTime GetTourTime() {
        return _key.Time;
    }

    public int GetRemainingCapacity()
    {
        return _capacity - _participants.Count;
    }

    public string GetGuide() {
        return _key.Guide;
    }

    public void SetGuide(Employee guide)
    {
        _key.Guide = guide.GetId();
    }

    public ICollection<Guest> GetParticipants() {
        return _participants;
    }

    public int GetCapacity() {
        return _capacity;
    }

    public TourCompositeKey GetId()
    {
        return _key;
    }
}