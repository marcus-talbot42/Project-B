using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public interface IEmployeeService : IService<Employee>
{
    Employee? FindValidEmployeeByUsernameAndPassword(string username, string password);
}