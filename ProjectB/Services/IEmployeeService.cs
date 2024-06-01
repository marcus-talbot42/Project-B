using ProjectB.Models;

namespace ProjectB.Services;

public interface IEmployeeService : IService<Employee>
{
    Employee? FindValidEmployeeByUsernameAndPassword(string username, string password);
}