using ProjectB.Models;
using ProjectB.Repositories;
using ProjectB.Settings;

namespace ProjectB.Services;

public class TranslationService(ITranslationRepository repository) : AbstractService<Translation>(repository), ITranslationService
{
    public Language Language { get; set; } = Language.NL;

    public string? GetTranslationString(string key)
    {
        var translation = repository.FindByKeyAndLanguage(key, Language);

        if (translation == null)
        {
            return $"Language not found: {Language}:{key}";
        }

        return translation.Value;
    }
}