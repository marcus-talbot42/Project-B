using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

/// <summary>
/// Represents a service for managing employees.
/// </summary>
public class EmployeeService(EmployeeRepository repository) : AbstractService<Employee>(repository), IEmployeeService
{
    /// <summary>
    /// Finds a valid employee by username.
    /// </summary>
    /// <param name="username">The username of the employee to find.</param>
    /// <returns>The found employee if it exists; otherwise, null.</returns>
    /// <exception cref="EntityNotFoundException">Thrown if an employee with the specified username is not found.</exception>
    public Employee? FindValidEmployeeByUsernameAndPassword(string username, string password)
    {
        Employee employee = repository.FindByUsernameAndPassword(username, password);
        if (employee == null)
        {
            throw new EntityNotFoundException($"Employee with username {username} not found.");
        }

        return employee;
    }
}