using System;
using ProjectB.Services; // Import the namespace where IService is defined

class Program
{
    static void Main(string[] args)
    {
        // Create an instance of the service
        IService service = new Service();

        // Call methods on the service
        service.DoSomething();

        // Rest of your code...
    }
}