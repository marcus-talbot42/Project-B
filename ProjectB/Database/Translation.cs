using ProjectB.Enums;

namespace ProjectB.Models;

public class Translation(string key, string value, Language language) : AbstractEntity
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
    public Language Language { get; set; } = language;
}