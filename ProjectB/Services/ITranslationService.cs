using ProjectB.Models;

namespace ProjectB.Services;

public interface ITranslationService : IService<Translation>
{
    string? GetTranslationString(string key);
}