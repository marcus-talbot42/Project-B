using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectB.Client;
using ProjectB.Database;
using ProjectB.Repositories;
using ProjectB.Services;
using ProjectB.Workflows.EmployeeFlows;
using ProjectB.Workflows.GuestFlows;
using Spectre.Console;

namespace ProjectB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new DepotClient(GetServices()).Run();
        }

        private static IServiceProvider GetServices()
        {
            return new ServiceCollection()
                .AddDbContext<IDatabaseContext, DatabaseContext>(options => options.UseInMemoryDatabase("ProjectBDatabase"))
                .AddSingleton<IAnsiConsole>(AnsiConsole.Console)

                // Repostitories
                .AddSingleton<IGuestRepository, GuestRepository>()
                .AddSingleton<IEmployeeRepository, EmployeeRepository>()
                .AddSingleton<ITourRepository, TourRepository>()
                .AddSingleton<ITranslationRepository, TranslationRepository>()

                // Services
                .AddSingleton<IDateTimeService, DateTimeService>()
                .AddSingleton<IEmployeeService, EmployeeService>()
                .AddSingleton<IGuestService, GuestService>()
                .AddSingleton<ITourService, TourService>()
                .AddSingleton<ITranslationService, TranslationService>()
                .AddSingleton<IPromptService, PromptService>()

                // Flows
                .AddTransient<CreateReservationFlow>()
                .AddTransient<EditReservationFlow>()
                .AddTransient<DeleteReservationFlow>()

                .AddTransient<AddGuestFlow>()
                .AddTransient<RemoveGuestFlow>()
                .AddTransient<StartTourFlow>()


                .BuildServiceProvider();
        }
    }
}