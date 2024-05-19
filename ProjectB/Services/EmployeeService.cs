using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

/// <summary>
/// Represents a service for managing employees.
/// </summary>
public class EmployeeService(EmployeeRepository repository) : IService<Employee, long>
{
    
    /// <summary>
    /// Creates a new employee.
    /// </summary>
    /// <param name="entity">The employee entity to create.</param>
    /// <exception cref="PrimaryKeyConstraintException">Thrown if an employee with the same username already exists.</exception>
    public void Create(Employee entity)
    {
        if (repository.Exists(entity))
        {
            throw new PrimaryKeyConstraintException($"Employee with username {entity.GetId()} already exists.");
        }

        repository.Save(entity);
    }

    /// <summary>
    /// Updates the information of an employee identified by the specified id.
    /// </summary>
    /// <param name="entity">The updated employee entity.</param>
    /// <param name="id">The id of the employee to update.</param>
    /// <exception cref="ArgumentNullException">Thrown if the entity is null.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the id is null.</exception>
    public void Update(Employee entity, long id)
    {
        repository.Save(entity);
    }

    /// <summary>
    /// Deletes an employee identified by the specified id.
    /// </summary>
    /// <param name="id">The id of the employee to delete.</param>
    public void Delete(long id)
    {
        repository.Remove(id);
    }

    /// <summary>
    /// Finds a valid employee by username.
    /// </summary>
    /// <param name="username">The username of the employee to find.</param>
    /// <returns>The found employee if it exists; otherwise, null.</returns>
    /// <exception cref="EntityNotFoundException">Thrown if an employee with the specified username is not found.</exception>
    public Employee? FindValidEmployeeByUsername(string username)
    {
        Employee employee = repository.FindByUsername(username);
        if (employee == null)
        {
            throw new EntityNotFoundException($"Employee with username {username} not found.");
        }

        return employee;
    }
    public Employee Read(long read)
    {
        throw new NotImplementedException();
    }
}