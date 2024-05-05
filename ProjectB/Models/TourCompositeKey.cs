using Newtonsoft.Json;
using ProjectB.Exceptions;

namespace ProjectB.Models;

public class TourCompositeKey {

    [JsonProperty] public readonly DateTime Time;
    [JsonProperty] public string Guide;

    public TourCompositeKey(DateTime time, string guide) {
        Guide = guide;
        Time = time;
    }
}