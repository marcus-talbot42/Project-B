using ProjectB.Database;

namespace ProjectB.Workflows
{
    public abstract class AbstractWorkflow(IDatabaseContext context)
    {
        public virtual (bool Success, string MessageKey) Commit()
        {
            var changes = context.SaveChanges();

            return (changes > 0, changes > 0 ?  "changesSaved" : "noChangesToSave");
        }
    }
}
