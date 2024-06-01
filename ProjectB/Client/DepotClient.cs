using Microsoft.Extensions.DependencyInjection;
using ProjectB.Choices;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Settings;
using Spectre.Console;

namespace ProjectB.Client
{
    public class DepotClient : IDepotClient
    {
        private IServiceProvider ServiceProvider { get; }
        protected IDatabaseContext Context { get; }
        protected IAnsiConsole Console { get; }
        protected IPromptService Prompts { get; }
        protected ITranslationService Translation { get; }
        protected IGuestService GuestService { get; }
        protected IEmployeeService EmployeeService { get; }
        protected ITourService TourService { get; }

        // Client state
        protected Guest? Guest { get; set; }
        protected Employee? Employee { get; set; }

        // Services
        protected IDateTimeService DateTime { get; }

        protected bool IsRunning { get; set; } = true;
        protected MenuLevel CurrentMenu { get; set; } = MenuLevel.MainMenu;

        public DepotClient(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            Console = ServiceProvider.GetService<IAnsiConsole>()!;
            Context = ServiceProvider.GetService<IDatabaseContext>()!;
            DateTime = ServiceProvider.GetService<IDateTimeService>()!;
            Prompts = ServiceProvider.GetService<IPromptService>()!;
            Translation = ServiceProvider.GetService<ITranslationService>()!;
            GuestService = ServiceProvider.GetService<IGuestService>()!;
            EmployeeService = ServiceProvider.GetService<IEmployeeService>()!;
            TourService = ServiceProvider.GetService<ITourService>()!;
        }

        public void Run()
        {

            AnsiConsole.Status().Start(Translation.GetTranslationString("wait"), ctx =>
            {
                ctx.Spinner(Spinner.Known.Material);
                ctx.Status($"\n {Translation.GetTranslationString("loadingData")}");
            });


            while (IsRunning && CurrentMenu >= MenuLevel.MainMenu)
            {
                AnsiConsole.Clear();
                var options = new List<NamedChoice<Action>>
                {
                    new NamedChoice<Action>($"[blue]{Translation.GetTranslationString("loginGuest")}[/]", ShowGuestLogin),
                    new NamedChoice<Action>($"[blue]{Translation.GetTranslationString("loginEmployee")}[/]", ShowEmployeeLogin),
                    new NamedChoice<Action>($"[blue]{Translation.GetTranslationString("switchLanguage")}[/]", ShowLanguageSwitcher),
                    new NamedChoice<Action>($"[blue]{Translation.GetTranslationString("debug")}[/]", ShowDebugMenu)
                };

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        public void ShowGuestLogin()
        {
            AnsiConsole.MarkupLine($"[blue]{Translation.GetTranslationString("enterTicketNumber")}[/]");

            Guest = null;
            while (Guest == null)
            {
                string ticketNumber = Prompts.AskTicketNumber("ticketNumber");
                Guest = GuestService.FindValidGuestById(ticketNumber);

                if (Guest == null)
                {
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine($"[red]{Translation.GetTranslationString("ticketNotFound")}[/]");
                }
            }

            if (Guest != null)
                ShowGuestMenu();
        }

        private void ShowGuestMenu()
        {
            CurrentMenu = MenuLevel.SubMenu;


            while (IsRunning && CurrentMenu >= MenuLevel.SubMenu)
            {
                AnsiConsole.Clear();
                var options = new List<NamedChoice<Action>>
                {
                    new NamedChoice<Action>($"[blue]{Translation.GetTranslationString("createReservationView")}[/]", BeginCreateReservation),
                    new NamedChoice<Action>($"[blue]{Translation.GetTranslationString("editReservationView")}[/]", BeginEditReservation),
                    new NamedChoice<Action>($"[blue]{Translation.GetTranslationString("deleteReservationView")}[/]", BeginDeleteReservation),
                    new NamedChoice<Action>($"[blue]{Translation.GetTranslationString("exit")}[/]", ExitSubMenu)
                };

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        private void BeginCreateReservation()
        {
            var options = TourService.GetAllToursTodayAfterNow().
                Select(tour => new NamedChoice<Tour>($"{tour.Start.Hour:00}:{tour.Start.Minute:00} - {TourService.GetRemainingCapacity(tour)}", tour));

            var selectedTour = Prompts.AskTour("chooseTour", options);

            // Register user for tour
            TourService.RegisterGuestForTour(GuestService.FindValidGuestById(Guest!.Username), selectedTour);
        }

        private void BeginEditReservation()
        {
            throw new NotImplementedException();
        }

        private void BeginDeleteReservation()
        {
            throw new NotImplementedException();
        }

        public void ShowEmployeeLogin()
        {
            AnsiConsole.MarkupLine($"[blue]{Translation.GetTranslationString("employeeLoginText")}[/]");

            Employee = null;
            while (Employee == null)
            {
                string username = Prompts.AskUsername("username");
                string password = Prompts.AskPassword("password");

                Employee = EmployeeService.FindValidEmployeeByUsernameAndPassword(username, password);
                Console.MarkupLine(Translation.GetTranslationString(Employee == null ? "loginFailed" : "loginSuccess"));
            }

            if (Employee != null)
                ShowEmployeeMenu();
        }

        private void ShowEmployeeMenu()
        {
            throw new NotImplementedException();
        }

        public void ShowLanguageSwitcher()
        {
            AnsiConsole.Clear(); // Clear the console screen

            Translation.Language = Prompts.AskLanguage("chooseOption");
        }

        public void ShowDebugMenu()
        {
            CurrentMenu = MenuLevel.SubMenu;


            while (IsRunning && CurrentMenu >= MenuLevel.SubMenu)
            {
                AnsiConsole.Clear();
                var options = new List<NamedChoice<Action>>
                {
                    new NamedChoice<Action>($"[blue]{Translation.GetTranslationString("createGuest")}[/]", ShowCreateGuest),
                    new NamedChoice<Action>($"[blue]{Translation.GetTranslationString("createEmployee")}[/]", ShowCreateEmployee),
                    new NamedChoice<Action>($"[blue]{Translation.GetTranslationString("exit")}[/]", ExitSubMenu)
                };

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        private void ShowCreateGuest()
        {
            string ticketNummer = Prompts.AskTicketNumber("ticketNumber");
            Console.WriteLine("Geef de geldigheidsdatum van het ticket in (yyyy-mm-dd): ");
            DateOnly validForDate = Prompts.AskDate("askValidityDate");

            GuestService.Add(new Guest() { Username = ticketNummer, ValidDate = validForDate });
        }

        private void ShowCreateEmployee()
        {
            string username = Prompts.AskUsername("username");
            string password = Prompts.AskPassword("password");
            UserRole role = Prompts.AskRole("chooseUserRole");

            // Create new employee
            EmployeeService.Add(new Employee() { Username = username, Role = role, Password = password });
            EmployeeService.SaveChanges();
        }

        private void ExitSubMenu()
        {
            CurrentMenu = MenuLevel.MainMenu;
        }
    }
}
