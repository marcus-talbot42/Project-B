using Microsoft.Extensions.DependencyInjection;
using ProjectB.Choices;
using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Enums;
using Spectre.Console;
using System.Threading.Channels;
using Microsoft.Extensions.Options;

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
            AnsiConsole.Status().Start(Translation.Get("wait"), ctx =>
            {
                ctx.Spinner(Spinner.Known.Material);
                ctx.Status($"\n {Translation.Get("loadingData")}");
            });


            while (IsRunning && CurrentMenu >= MenuLevel.MainMenu)
            {
                AnsiConsole.Clear();
                var options = new List<NamedChoice<Action>>
                {
                    new NamedChoice<Action>($"[blue]{Translation.Get("loginGuest")}[/]", ShowGuestLogin),
                    new NamedChoice<Action>($"[blue]{Translation.Get("loginEmployee")}[/]", ShowEmployeeLogin),
                    new NamedChoice<Action>($"[blue]{Translation.Get("switchLanguage")}[/]", ShowLanguageSwitcher),
                    new NamedChoice<Action>($"[blue]{Translation.Get("debug")}[/]", ShowDebugMenu)
                };

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        public void ShowGuestLogin()
        {
            AnsiConsole.MarkupLine($"[blue]{Translation.Get("enterTicketNumber")}[/]");

            Guest = null;
            while (Guest == null)
            {
                string ticketNumber = Prompts.AskTicketNumber("ticketNumber");
                Guest = GuestService.FindValidGuestById(ticketNumber);

                if (Guest == null)
                {
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine($"[red]{Translation.Get("ticketNotFound")}[/]");
                }
            }

            if (Guest != null)
                ShowGuestMenu();
        }

        public void ShowEmployeeLogin()
        {
            AnsiConsole.MarkupLine($"[blue]{Translation.Get("employeeLoginText")}[/]");

            Employee = null;
            while (Employee == null)
            {
                string username = Prompts.AskUsername("username");
                string password = Prompts.AskPassword("password");

                Employee = EmployeeService.FindValidEmployeeByUsernameAndPassword(username, password);
                Console.MarkupLine(Translation.Get(Employee == null ? "loginFailed" : "loginSuccess"));
            }

            if (Employee != null)
                ShowEmployeeMenu();
        }

        #region GuestMenu

        private void ShowGuestMenu()
        {
            CurrentMenu = MenuLevel.SubMenu;

            while (IsRunning && CurrentMenu >= MenuLevel.SubMenu)
            {
                AnsiConsole.Clear();

                var options = new List<NamedChoice<Action>>();

                var currentTour = TourService.GetTourForGuest(Guest!);
                if (currentTour == null)
                {
                    options.Add(new NamedChoice<Action>($"[blue]{Translation.Get("createReservationView")}[/]", BeginCreateReservation));
                }
                else
                {
                    options.Add(new NamedChoice<Action>($"[blue]{Translation.Get("editReservationView")}[/]", BeginEditReservation));
                    options.Add(new NamedChoice<Action>($"[blue]{Translation.Get("deleteReservationView")}[/]", BeginDeleteReservation));
                }
                options.Add(new NamedChoice<Action>($"[blue]{Translation.Get("exit")}[/]", () => { ExitToMenu(MenuLevel.MainMenu); }));

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        private void BeginCreateReservation()
        {
            var options = TourService.GetAllToursTodayAfterNow().
                Select(tour => new NamedChoice<Tour>(Translation.GetReplacement("tourOption", new() { tour.Start.ToShortTimeString(), TourService.GetRemainingCapacity(tour).ToString() }), tour));

            var selectedTour = Prompts.AskTour("chooseTour", options);

            Console.MarkupLine(Translation.GetReplacement("confirmReservation", new() { selectedTour.Start.ToShortTimeString() }));
            if (Prompts.AskYesNo("confirmYesNo", "yes", "no"))
            {
                if (Guest != null && selectedTour != null)
                    TourService.RegisterGuestForTour(Guest!, selectedTour);
            }

            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void BeginEditReservation()
        {
            var currentTour = TourService.GetTourForGuest(Guest!);
            if (currentTour == null)
            {
                Console.MarkupLine(Translation.Get("noReservationFound"));
                Prompts.ShowSpinner("returningToMenu", 2000);
                return;
            }

            Console.MarkupLine(Translation.GetReplacement("currentReservation", new() { currentTour.Start.ToShortTimeString() }));
            Console.MarkupLine(Translation.Get("confirmEditReservation"));
            if (!Prompts.AskYesNo("confirmYesNo", "yes", "no"))
            {
                Prompts.ShowSpinner("returningToMenu", 2000);
                return;
            }

            var options = TourService.GetAllToursTodayAfterNow().
                Select(tour => new NamedChoice<Tour>(Translation.GetReplacement("tourOption", new() { tour.Start.ToShortTimeString(), TourService.GetRemainingCapacity(tour).ToString() }), tour));

            var selectedTour = Prompts.AskTour("chooseTour", options);

            Console.MarkupLine(Translation.GetReplacement("confirmEditReservationChoice", new() { selectedTour.Start.ToShortTimeString() }));
            if (Prompts.AskYesNo("confirmYesNo", "yes", "no"))
            {
                if (Guest != null && selectedTour != null)
                {
                    TourService.EditRegistrationGuestForTour(Guest!, selectedTour);
                }
            }

            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void BeginDeleteReservation()
        {
            var currentTour = TourService.GetTourForGuest(Guest!);
            if (currentTour == null)
            {
                Console.MarkupLine(Translation.Get("noReservationFound"));
                Prompts.ShowSpinner("returningToMenu", 2000);
                return;
            }

            Console.MarkupLine(Translation.GetReplacement("currentReservation", new() { currentTour.Start.ToShortTimeString() }));
            Console.MarkupLine(Translation.Get("confirmDeleteReservation"));
            if (!Prompts.AskYesNo("confirmYesNo", "yes", "no"))
            {
                Prompts.ShowSpinner("returningToMenu", 2000);
                return;
            }

            if (Guest != null)
                TourService.DeleteReservationGuest(Guest!);

            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        #endregion

        private void ShowEmployeeMenu()
        {
            CurrentMenu = MenuLevel.SubMenu;

            while (IsRunning && CurrentMenu >= MenuLevel.SubMenu)
            {
                AnsiConsole.Clear();

                var options = TourService.GetAllToursTodayAfterNow(). // TODO: Add filter for employee
                    Select(tour => new NamedChoice<Action>(Translation.GetReplacement("employeeTourOption", new() { tour.Start.ToShortTimeString(), TourService.GetRemainingCapacity(tour).ToString() }), () => { ShowTourMenu(tour); })).ToList();

                options.Add(new NamedChoice<Action>($"[blue]{Translation.Get("exit")}[/]", () => { ExitToMenu(MenuLevel.MainMenu); }));

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        private void ShowTourMenu(Tour tour)
        {
            CurrentMenu = MenuLevel.ActionsMenu;

            while (IsRunning && CurrentMenu >= MenuLevel.ActionsMenu)
            {
                AnsiConsole.Clear();
                Console.MarkupLine(Translation.GetReplacement("tourMenu", new() { tour.Start.ToShortTimeString(), TourService.GetRemainingCapacity(tour).ToString() }));

                var options = new List<NamedChoice<Action>>
                {
                    new NamedChoice<Action>($"[blue]{Translation.Get("startTour")}[/]", () => { BeginStartTour(tour); }),
                    new NamedChoice<Action>($"[blue]{Translation.Get("addGuest")}[/]", () => { BeginAddGuest(tour); }),
                    new NamedChoice<Action>($"[blue]{Translation.Get("removeGuest")}[/]", () => { BeginRemoveGuest(tour); }),
                    new NamedChoice<Action>($"[blue]{Translation.Get("exit")}[/]", () => { ExitToMenu(MenuLevel.SubMenu); })
                };

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        private void BeginStartTour(Tour tour)
        {
            Console.MarkupLine(Translation.GetReplacement("employeeTourInfo", new() { tour.Start.ToShortTimeString() }));
            Console.WriteLine();

            Console.MarkupLine(Translation.Get("registeredGuests"));
            foreach (var guestNumber in tour.Participants)
                Console.MarkupLine(Translation.GetReplacement("guestList", new() { guestNumber }));
            Console.WriteLine();

            if (tour.EmployeeNumber != Employee!.EmployeeNumber)
            {
                Console.MarkupLine(Translation.Get("notYourTour"));
                if (!Prompts.AskYesNo("confirmYesNo", "yes", "no"))
                {
                    Prompts.ShowSpinner("returningToMenu", 2000);
                    return;
                }

                tour.EmployeeNumber = Employee.EmployeeNumber;
                TourService.SaveChanges();
            }

            Console.MarkupLine(Translation.Get("scanAllTickets"));

            List<int> scannedTickets = new List<int>();

            bool continueScanning = true;
            while (scannedTickets.Count != tour.Participants.Count && continueScanning)
            {
                if (!int.TryParse(Prompts.AskTicketOrEmployeeNumber("ticketNumberOrEmployeeNumber"), out int number))
                    continue;

                if (number < 10000000)
                {
                    Console.MarkupLine(Translation.Get("finishedScanningQuestion"));
                    if (Prompts.AskYesNo("confirmYesNo", "yes", "no"))
                    {
                        continueScanning = false;
                        continue;
                    }
                }

                if (scannedTickets.Contains(number))
                {
                    Console.MarkupLine(Translation.Get("ticketAlreadyScanned"));
                    continue;
                }
                
                if(!tour.Participants.Contains(number.ToString()))
                {
                    Console.MarkupLine(Translation.Get("ticketNotInTour"));
                    continue;
                }

                scannedTickets.Add(number);
            }

            Console.MarkupLine(Translation.Get("finishedScanning"));

            tour.Participants = scannedTickets.Select(number => number.ToString()).ToList();

            continueScanning = true;
            while (tour.Participants.Count < 13 && continueScanning)
            {
                if (!int.TryParse(Prompts.AskTicketOrEmployeeNumber("ticketNumberOrEmployeeNumber"), out int number))
                    continue;

                if (number < 10000000)
                {
                    Console.MarkupLine(Translation.Get("finishedScanningQuestion"));
                    if (Prompts.AskYesNo("confirmYesNo", "yes", "no"))
                    {
                        continueScanning = false;
                        continue;
                    }
                }

                if (scannedTickets.Contains(number))
                {
                    Console.MarkupLine(Translation.Get("ticketAlreadyScanned"));
                    continue;
                }

                scannedTickets.Add(number);
            }

            Console.MarkupLine(Translation.Get("finishedScanningExtraTickets"));

            tour.Participants = scannedTickets.Select(number => number.ToString()).ToList();

            Console.MarkupLine(Translation.Get("confirmStartTour"));
            if (!Prompts.AskYesNo("confirmYesNo", "yes", "no"))
            {
                Prompts.ShowSpinner("returningToMenu", 2000);
                return;
            }

            tour.Departed = true;
            TourService.SaveChanges();

            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void BeginAddGuest(Tour tour)
        {
            Console.MarkupLine(Translation.GetReplacement("employeeTourInfo", new() { tour.Start.ToShortTimeString() }));
            Console.WriteLine();

            if (tour.Participants.Count == 13)
            {
                Console.MarkupLine(Translation.Get("noSpaceInTour"));
                Prompts.ShowSpinner("returningToMenu", 2000);
                return;
            }

            string ticketNumber = Prompts.AskTicketNumber("ticketNumber");
            var guest = GuestService.FindValidGuestById(ticketNumber);

            var currentTour = TourService.GetTourForGuest(Guest!);
            if (currentTour != null)
            {
                Console.MarkupLine(Translation.Get("guestAlreadyHasReservation"));
                Prompts.ShowSpinner("returningToMenu", 2000);
                return;
            }

            Console.MarkupLine(Translation.GetReplacement("confirmAddGuest", new() { tour.Start.ToShortTimeString() }));
            if (Prompts.AskYesNo("confirmYesNo", "yes", "no"))
            {
                if (guest != null && tour != null)
                    TourService.RegisterGuestForTour(guest, tour);
            }

            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void BeginRemoveGuest(Tour tour)
        {
            Console.MarkupLine(Translation.GetReplacement("employeeTourInfo", new() { tour.Start.ToShortTimeString() }));
            Console.WriteLine();

            Console.MarkupLine(Translation.Get("registeredGuests"));
            foreach (var guestNumber in tour.Participants)
                Console.MarkupLine(Translation.GetReplacement("guestList", new() { guestNumber }));
            Console.WriteLine();

            string ticketNumber = Prompts.AskTicketNumber("ticketNumberToRemove");
            var guest = GuestService.FindValidGuestById(ticketNumber);
            if (guest == null || !tour.Participants.Contains(guest.TicketNumber))
            {
                Console.MarkupLine(Translation.Get("guestNotFoundInTour"));
                Prompts.ShowSpinner("returningToMenu", 2000);
                return;
            }

            Console.MarkupLine(Translation.Get("confirmGuestRemoval"));
            if (!Prompts.AskYesNo("confirmYesNo", "yes", "no"))
            {
                Prompts.ShowSpinner("returningToMenu", 2000);
                return;
            }

            if (Guest != null)
                TourService.DeleteReservationGuest(guest);

            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        #region LanguageSwitcher

        public void ShowLanguageSwitcher()
        {
            AnsiConsole.Clear(); // Clear the console screen

            Translation.Language = Prompts.AskLanguage("chooseOption");
        }

        #endregion

        #region DebugMenu

        public void ShowDebugMenu()
        {
            CurrentMenu = MenuLevel.SubMenu;


            while (IsRunning && CurrentMenu >= MenuLevel.SubMenu)
            {
                AnsiConsole.Clear();
                var options = new List<NamedChoice<Action>>
                {
                    new NamedChoice<Action>($"[blue]{Translation.Get("createGuest")}[/]", ShowCreateGuest),
                    new NamedChoice<Action>($"[blue]{Translation.Get("createEmployee")}[/]", ShowCreateEmployee),
                    new NamedChoice<Action>($"[blue]{Translation.Get("createTours")}[/]", ShowCreateTours),
                    new NamedChoice<Action>($"[blue]{Translation.Get("createBulkTickets")}[/]", ShowCreateBulkTickets),
                    new NamedChoice<Action>($"[blue]{Translation.Get("exit")}[/]", () => { ExitToMenu(MenuLevel.MainMenu); })
                };

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        private void ShowCreateBulkTickets()
        {
            GuestService.AddRange(new List<int> { 11245178, 12398476, 12439876, 12837465, 12938476, 12948736, 15328746, 15938274, 16749283, 23489716, 27381946, 27639481, 28371946, 35921847,
                    39271584, 39421587, 39457821, 45839217, 46827391, 48627193, 48675913, 57214836, 57239186, 57264318, 57936214, 61827394, 62839174, 63821974, 68739214, 73189245, 73918245,
                    78439215, 79358124, 81479625, 89231546, 89231746, 89237416, 91328746, 91472618, 91572618, 94726183, 18954201, 17541254, 16544824, 13548424, 15426158, 14684472 }
                .Select(ticket => new Guest() { TicketNumber = ticket.ToString(), Role = UserRole.Guest, ValidDate = DateOnly.FromDateTime(DateTime.Now), Expires = false }).ToList());
            int changes = GuestService.SaveChanges();

            Console.MarkupLine(Translation.GetReplacement("changesSaved", new() { changes.ToString() }));
            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void ShowCreateTours()
        {
            int tourDuration = 40;

            DateOnly startDate = Prompts.AskDate("askStartDate");
            DateOnly endDate = Prompts.AskDate("askEndDate");
            if (startDate > endDate)
            {
                Console.MarkupLine(Translation.Get("startAfterEndDate"));
                return;
            }

            TimeOnly startTime = Prompts.AskTime("askStartTime");
            TimeOnly endTime = Prompts.AskTime("askEndTime");
            if (startTime > endTime)
            {
                Console.MarkupLine(Translation.Get("startAfterEndTime"));
                return;
            }

            int interval = Prompts.AskNumber("askInterval", 1, 60);

            var planning = new List<Tour>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
                for (var time = startTime; time.Add(TimeSpan.FromMinutes(tourDuration)) <= endTime; time = time.Add(TimeSpan.FromMinutes(interval)))
                    planning.Add(new Tour(new DateTime(date, time)));

            if (planning.Count > 0)
            {
                TourService.AddRange(planning);
                int changes = TourService.SaveChanges();

                Console.MarkupLine(Translation.GetReplacement("changesSaved", new() { changes.ToString() }));
            }
            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void ShowCreateGuest()
        {
            string ticketNumber = Prompts.AskTicketNumber("ticketNumber");
            DateOnly validForDate = Prompts.AskDate("askValidityDate");

            GuestService.Add(new Guest() { TicketNumber = ticketNumber, ValidDate = validForDate });
            int changes = GuestService.SaveChanges();

            Console.MarkupLine(Translation.GetReplacement("changesSaved", new() { changes.ToString() }));
            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void ShowCreateEmployee()
        {
            string username = Prompts.AskUsername("username");
            string password = Prompts.AskPassword("password");
            string employeeNumber = Prompts.AskNumber("employeeNumber",1, 9999999).ToString();
            UserRole role = Prompts.AskRole("chooseUserRole");

            // Create new employee
            EmployeeService.Add(new Employee() { Username = username, Role = role, Password = password, EmployeeNumber = employeeNumber });
            int changes = EmployeeService.SaveChanges();

            Console.MarkupLine(Translation.GetReplacement("changesSaved", new() { changes.ToString() }));
            Prompts.ShowSpinner("returningToMenu", 2000);
        }
        #endregion

        #region Shared

        private void ExitToMenu(MenuLevel menuLevel = MenuLevel.MainMenu)
        {
            CurrentMenu = menuLevel;
        }

        #endregion
    }
}
