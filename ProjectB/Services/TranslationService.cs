using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class TranslationService(TranslationRepository repository) : IService<Translation, long>
{
    public void Create(Translation entity)
    {
        repository.Save(entity);
    }

    public void Update(Translation entity, long id)
    {
        repository.Save(entity);
    }

    public void Delete(long id)
    {
        repository.Remove(id);
    }

    public Translation Read(long key)
    {
        return repository.FindById(key)!;
    }


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