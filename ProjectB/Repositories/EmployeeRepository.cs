using ProjectB.Database;
using ProjectB.Models;

namespace ProjectB.Repositories;

public class EmployeeRepository(DatabaseContext context) : AbstractRepository<Employee>(context)
{
    
    public Employee? FindByUsername(string username) =>
        (from user in DbSet where user.GetUsername() == username select user).FirstOrDefault();
}