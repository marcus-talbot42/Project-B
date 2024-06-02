using ProjectB.Models;
using ProjectB.Enums;

namespace ProjectB.Services;

public interface ITranslationService : IService<Translation>
{
    public Language Language { get; set; }

    string GetTranslationString(string key);
}