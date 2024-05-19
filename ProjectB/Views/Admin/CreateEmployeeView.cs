using ProjectB.Models;
using ProjectB.Services;

namespace ProjectB.Views.Admin;

public class CreateEmployeeView(EmployeeService employeeService) : AbstractView
{
    public override void Output()
    {
        // Get user input from console to determine the username
        Console.WriteLine("Enter the username:");
        string username = Console.ReadLine()!;
        
        // Get user input from console, to determine the password of the user
        Console.WriteLine("Enter the password:");
        string password = Console.ReadLine()!;
        
        // Get user input from console, to determine the UserRole
        Console.WriteLine("Enter the user role (Guest, Guide, or DepartmentHead):");
        UserRole role;
        Enum.TryParse(Console.ReadLine(), out role);
        
        // Create new employee
        employeeService.Add(new Employee() { Username= username, Role = role, Password = password });
        employeeService.SaveChanges();
    }
}