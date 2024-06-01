using ProjectB.Database;
using ProjectB.Models;

namespace ProjectB.Repositories;

public class EmployeeRepository(IDatabaseContext context) : AbstractRepository<Employee>(context), IEmployeeRepository
{
    public Employee? FindByUsernameAndPassword(string username, string password) =>
        (from user in DbSet
         where user.Username == username && BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password, BCrypt.Net.HashType.SHA384)
         select user).FirstOrDefault();
}