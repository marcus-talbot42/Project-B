using ProjectB.Models;
using ProjectB.Repositories;
using ProjectB.Enums;

namespace ProjectB.Services;

public class TranslationService(ITranslationRepository repository) : AbstractService<Translation>(repository), ITranslationService
{
    public Language Language { get; set; } = Language.NL;

    public string Get(string key)
    {
        var translation = repository.FindByKeyAndLanguage(key, Language);

        if (translation == null)
            return $"Language not found: {Language}:{key}";

        return translation.Value;
    }

    public string GetReplacement(string key, List<string> replacements)
    {
        var translation = Get(key);

        for (var i = 0; i < replacements.Count; i++)
            translation = translation.Replace($"{{{i}}}", replacements[i]);

        return translation;
    }
}