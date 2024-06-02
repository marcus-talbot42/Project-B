using ProjectB.Database;
using ProjectB.Enums;
using ProjectB.Models;

namespace ProjectB.Repositories;

public class TranslationRepository(IDatabaseContext context) : AbstractRepository<Translation>(context), ITranslationRepository
{
    public Translation FindByKeyAndLanguage(string key, Language language) => (from translation in DbSet.ToList()
                                                                               where translation.Language == language && translation.Key == key
                                                                               select translation).FirstOrDefault();
}