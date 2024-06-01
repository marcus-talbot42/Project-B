using ProjectB.Models;
using ProjectB.Settings;

namespace ProjectB.Services;

public interface ITranslationService : IService<Translation>
{
    public Language Language { get; set; }

    string? GetTranslationString(string key);
}