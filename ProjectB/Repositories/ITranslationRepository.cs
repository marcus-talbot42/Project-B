using ProjectB.Models;
using ProjectB.Settings;

namespace ProjectB.Repositories
{
    public interface ITranslationRepository : IRepository<Translation>
    {
        Translation FindByKeyAndLanguage(string key, Language language);
    }
}