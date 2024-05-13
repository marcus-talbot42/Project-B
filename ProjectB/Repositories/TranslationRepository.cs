using ProjectB.Models;

namespace ProjectB.Repositories;

public class TranslationRepository : AbstractRepository<Translation, string>
{
    public override string GetFileLocation()
    {
        return $".//../../../Database/{GetType().Name}.json";
    }
}