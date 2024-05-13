using ProjectB.Models;
using ProjectB.Repositories;
using ProjectB.settings;

namespace ProjectB.Services;

public class TranslationService(IRepository<Translation, string> repository) : IService<Translation, string>
{
    public void Create(Translation entity)
    {
        repository.Save(entity);
    }

    public void Update(Translation entity, string id)
    {
        repository.Save(entity);
    }

    public void Delete(string id)
    {
        repository.Remove(id);
    }

    public Translation Read(string key)
    {
        return repository.FindById(Settings.Lanuage.ToString())!;
    }


    public string GetTranslationString(string key)
    {
        var lang = Settings.Lanuage.ToString();

        var translation = repository.FindById(lang);

        if (translation == null)
        {
            return $"Lanuage not found: {lang}";
        }

        if (!translation.GetPairs().ContainsKey(key))
        {
            return $"Translation not found: {key}";
        }

        return translation.GetPairs()[key];
    }
}