using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class EmployeeService : IService<Employee, string>
{

    private static readonly string FakePasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("FakePasswordToPreventUserEnumerationExploit");
    private static readonly Lazy<EmployeeService> Lazy = new(() => new EmployeeService());
    public static EmployeeService Instance => Lazy.Value;
    
    private readonly EmployeeRepository _repository = EmployeeRepository.Instance;
    
    public void Create(Employee entity)
    {
        throw new NotImplementedException();
    }

    public void Update(Employee entity, string id)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public Employee? Authenticate(string username, string password)
    {
        if (!_repository.Exists(username))
        {
            BCrypt.Net.BCrypt.EnhancedVerify(password, FakePasswordHash);
            return null;
        }

        Employee employee = _repository.FindById(username);
        if (employee.IsPasswordCorrect(password))
        {
            return employee;
        }

        return null;
    }
}