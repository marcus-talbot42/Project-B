using ProjectB.Enums;
using ProjectB.Models;

namespace ProjectB.Repositories
{
    public interface ITranslationRepository : IRepository<Translation>
    {
        Translation FindByKeyAndLanguage(string key, Language language);
    }
}