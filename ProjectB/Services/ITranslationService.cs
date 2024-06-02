using ProjectB.Enums;
using ProjectB.Models;

namespace ProjectB.Services;

public interface ITranslationService : IService<Translation>
{
    public Language Language { get; set; }

    string Get(string key);

    string GetReplacement(string key, List<string> replacements);
}