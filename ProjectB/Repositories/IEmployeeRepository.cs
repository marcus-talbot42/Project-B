using ProjectB.Models;

namespace ProjectB.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee? FindByUsernameAndPassword(string username, string password);
    }
}