using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class TranslationService(TranslationRepository repository) : AbstractService<Translation>(repository), ITranslationService
{
    public string? GetTranslationString(string key)
    {
        var translation = repository.FindByKeyAndLanguage(key, Settings.Settings.Language);

        if (translation == null)
        {
            return $"Language not found: {Settings.Settings.Language}:{key}";
        }

        return translation.Value;
    }
}