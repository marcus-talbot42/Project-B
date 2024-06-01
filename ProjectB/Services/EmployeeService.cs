using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

/// <summary>
/// Represents a service for managing employees.
/// </summary>
public class EmployeeService(IEmployeeRepository repository) : AbstractService<Employee>(repository), IEmployeeService
{
    /// <summary>
    /// Finds a valid employee by username.
    /// </summary>
    /// <param name="username">The username of the employee to find.</param>
    /// <returns>The found employee if it exists; otherwise, null.</returns>
    public Employee? FindValidEmployeeByUsernameAndPassword(string username, string password) => repository.FindByUsernameAndPassword(username, password);
}