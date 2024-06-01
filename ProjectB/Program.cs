using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectB.Client;
using ProjectB.Database;
using ProjectB.Repositories;
using ProjectB.Services;
using ProjectB.Views.Admin;
using ProjectB.Views.Debug;
using ProjectB.Views.Language;
using ProjectB.Views.Login;
using ProjectB.Views.Main;
using ProjectB.Views.Reservation;
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

                // Views
                .AddTransient<CreateReservationView>()
                .AddTransient<EditReservationView>()
                .AddTransient<DeleteReservationView>()
                .AddTransient<ReservationView>()
                .AddTransient<EmployeeLoginView>()
                .AddTransient<GuestLoginView>()
                .AddTransient<CreateGuestView>()
                .AddTransient<CreateEmployeeView>()
                .AddTransient<DebugView>()
                .AddTransient<MainMenuView>()
                .AddTransient<LanguageSwitcher>()

                .BuildServiceProvider();
        }
    }
}