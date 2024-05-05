using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjectB.Models;

public class Tour : IEntity<TourCompositeKey>
{

    [JsonProperty] private readonly ICollection<string> _participants;
    [JsonProperty] private readonly int _capacity;
    [JsonProperty] private readonly TourCompositeKey _key;

    public Tour(TourCompositeKey key, int capacity, ICollection<string> participants)
    {
        _key = key;
        _capacity = capacity;
        _participants = participants;
    }

    public int RemainingCapacity => _capacity - _participants.Count;

    public DateTime GetTourTime() {
        return _key.Time;
    }

    public string GetGuide() {
        return _key.Guide;
    }

    public void SetGuide(Employee guide) {
        _key.Guide = guide.GetId();
    }

    public ICollection<string> GetParticipants() {
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