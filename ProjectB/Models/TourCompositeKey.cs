using Newtonsoft.Json;

namespace ProjectB.Models;

public class TourCompositeKey {

    [JsonProperty] public readonly DateTime Time;
    [JsonProperty] public string Guide { get; set; }

    public TourCompositeKey(DateTime time, string guide)
    {
        Guide = guide;
        Time = time;
    }
}