using ProjectB.Models;
using ProjectB.Enums;

namespace ProjectB.Repositories
{
    public interface ITranslationRepository : IRepository<Translation>
    {
        Translation FindByKeyAndLanguage(string key, Language language);
    }
}