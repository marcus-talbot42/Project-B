using ProjectB.Models;
using ProjectB.Enums;

namespace ProjectB.Services;

public interface ITranslationService : IService<Translation>
{
    public Language Language { get; set; }

    string Get(string key);

    string GetReplacement(string key, List<string> replacements);
}