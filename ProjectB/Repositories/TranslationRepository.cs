using ProjectB.Models;

namespace ProjectB.Repositories;

public class TranslationRepository : AbstractRepository<Translation, string>
{
    // not needed any more, will be deleted soon
    // public override string GetFileLocation()
    // {
    //     return $".//../../../Database/{GetType().Name}.json";
    // }
}