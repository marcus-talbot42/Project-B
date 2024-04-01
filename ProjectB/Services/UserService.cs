using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public class UserService: IService<AbstractUser, string>
{

    private static readonly Lazy<UserService> Lazy = new (() => new UserService());
    public static UserService Instance => Lazy.Value;

    private readonly UserRepository _repository;

    private UserService() => _repository = UserRepository.Instance;

    public void Test()
    {
        Console.WriteLine("Got the UserService...");
        Console.WriteLine(_repository.FindAll());
    }

    public void Create(AbstractUser entity)
    {
        if (_repository.FindById(entity.GetId()) != null)
        {
            throw new PrimaryKeyConstraintException($"Primary Key 'User._username' violated. A User with ID {entity.GetId()} already exists.");
        }
        _repository.Save(entity);
    }

    public void Update(AbstractUser entity, string id)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }
}