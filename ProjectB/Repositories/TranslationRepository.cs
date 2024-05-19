using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Settings;

namespace ProjectB.Repositories;

public class TranslationRepository(DatabaseContext context) : AbstractRepository<Translation, long>(context)
{
    public Translation FindByKeyAndLanguage(string key, Language language) => (from translation in DbSet.ToList()
        where translation.Language == language && translation.Key == key
        select translation).FirstOrDefault();
}