using ProjectB.Models;

namespace ProjectB.Repositories;

public class EmployeeRepository : AbstractRepository<Employee, string>
{

    private static readonly Lazy<EmployeeRepository> Lazy = new(() => new EmployeeRepository());
    public static EmployeeRepository Instance => Lazy.Value;

    private EmployeeRepository()
    {
    }

    public IEnumerable<Employee> FindAllByRole(UserRole role) =>
        from user in Repository.Values
        where user.GetUserRole() == role
        select user;
}