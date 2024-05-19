using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public interface ITranslationService : IService<Translation>
{
    string? GetTranslationString(string key);
}