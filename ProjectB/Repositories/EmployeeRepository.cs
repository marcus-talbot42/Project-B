using ProjectB.Database;
using ProjectB.Models;

namespace ProjectB.Repositories;

public class EmployeeRepository(DatabaseContext context) : AbstractRepository<Employee>(context)
{
    public Employee? FindByUsernameAndPassword(string username, string password) =>
        (from user in DbSet
            where user.Username == username && BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password)
            select user).FirstOrDefault();
}