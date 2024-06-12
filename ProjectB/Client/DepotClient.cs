using Microsoft.Extensions.DependencyInjection;
using ProjectB.Choices;
using ProjectB.Database;
using ProjectB.Enums;
using ProjectB.Models;
using ProjectB.Services;
using ProjectB.Workflows;
using ProjectB.Workflows.EmployeeFlows;
using ProjectB.Workflows.GuestFlows;
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
            AnsiConsole.Status().Start(Translation.Get("wait"), ctx =>
            {
                ctx.Spinner(Spinner.Known.Material);
                ctx.Status($"\n {Translation.Get("loadingData")}");
            });

            while (IsRunning && CurrentMenu >= MenuLevel.MainMenu)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"{Translation.Get("welcomeToMuseum")}");

                var options = new List<NamedChoice<Action>>
                {
                    new NamedChoice<Action>($"{Translation.Get("loginGuest")}", ShowGuestLogin),
                    new NamedChoice<Action>($"{Translation.Get("loginEmployee")}", ShowEmployeeLogin),
                    new NamedChoice<Action>($"{Translation.Get("switchLanguage")}", ShowLanguageSwitcher),
                    new NamedChoice<Action>($"{Translation.Get("debug")}", ShowDebugMenu),
                    new NamedChoice<Action>($"{Translation.Get("exit")}", () => { IsRunning = false; })
                };

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        public void ShowGuestLogin()
        {
            Print("enterTicketNumber");

            Guest = null;
            while (Guest == null)
            {
                string ticketNumber = Prompts.AskTicketNumber("ticketNumber");
                Guest = GuestService.FindValidGuestById(ticketNumber);

                if (Guest == null)
                {
                    AnsiConsole.Clear();
                    Print("ticketNotFound");
                }
            }

            if (Guest != null)
                ShowGuestMenu();
        }

        public void ShowEmployeeLogin()
        {
            Print("employeeLoginText");

            Employee = null;
            while (Employee == null)
            {
                string username = Prompts.AskUsername("username");
                string password = Prompts.AskPassword("password");

                Employee = EmployeeService.FindValidEmployeeByUsernameAndPassword(username, password);
                Print(Employee == null ? "loginFailed" : "loginSuccess");
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
                    options.Add(new NamedChoice<Action>($"{Translation.Get("createReservationView")}", BeginCreateReservation));
                }
                else
                {
                    options.Add(new NamedChoice<Action>($"{Translation.Get("editReservationView")}", BeginEditReservation));
                    options.Add(new NamedChoice<Action>($"{Translation.Get("deleteReservationView")}", BeginDeleteReservation));
                }
                options.Add(new NamedChoice<Action>($"{Translation.Get("logout")}", () => { ExitToMenu(MenuLevel.MainMenu); }));

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        private void BeginCreateReservation()
        {
            var flow = GetFlow<CreateReservationFlow>();

            if (!HandleFlowResult(flow.SetGuest(Guest!)))
                return;

            var options = TourService.GetAllToursTodayAfterNow().
                Select(tour => new NamedChoice<Tour>(Translation.GetReplacement("tourOption", new() { tour.Start.ToShortTimeString(), TourService.GetRemainingCapacity(tour).ToString() }), tour));

            if (!HandleFlowResult(flow.SetTour(Prompts.AskTour("chooseTour", options))))
                return;

            HandleFlowConfirmation(flow,
                TGet("confirmReservation", new() { flow.SelectedTour!.Start.ToShortTimeString() }),
                TGet("reservationSuccess", new() { flow.SelectedTour!.Start.ToShortTimeString() }),
                TGet("reservationCancelled"));

            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void BeginEditReservation()
        {
            var flow = GetFlow<EditReservationFlow>();

            if (!HandleFlowResult(flow.SetGuest(Guest!)))
                return;

            Print("currentReservation", new() { flow.CurrentTour!.Start.ToShortTimeString() });

            Print("confirmEditReservation");
            if (!HandleConfirmation())
                return;

            var options = TourService.GetAllToursTodayAfterNow().
                Select(tour => new NamedChoice<Tour>(Translation.GetReplacement("tourOption", new() { tour.Start.ToShortTimeString(), TourService.GetRemainingCapacity(tour).ToString() }), tour));


            if (!HandleFlowResult(flow.SetTour(Prompts.AskTour("chooseTour", options))))
                return;

            HandleFlowConfirmation(flow,
                TGet("confirmEditReservationChoice", new() { flow.SelectedTour!.Start.ToShortTimeString() }),
                TGet("reservationSuccess", new() { flow.SelectedTour!.Start.ToShortTimeString() }),
                TGet("reservationEditCancelled"));

            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void BeginDeleteReservation()
        {
            var flow = GetFlow<DeleteReservationFlow>();

            if (!HandleFlowResult(flow.SetGuest(Guest!)))
                return;

            Print("currentReservation", new() { flow.CurrentTour!.Start.ToShortTimeString() });

            HandleFlowConfirmation(flow,
                TGet("confirmDeleteReservation"),
                TGet("reservationDeleted"),
                TGet("reservationNotDeleted"));

            Prompts.ShowSpinner("returningToMenu", 2000);
        }
        #endregion

        #region EmployeeMenu
        private void ShowEmployeeMenu()
        {
            CurrentMenu = MenuLevel.SubMenu;

            while (IsRunning && CurrentMenu >= MenuLevel.SubMenu)
            {
                AnsiConsole.Clear();

                var options = new List<NamedChoice<Action>> { new NamedChoice<Action>($"{Translation.Get("logout")}", () => { ExitToMenu(MenuLevel.MainMenu); }) };
                options.AddRange(TourService.GetAllToursTodayAfterNow()
                    .Select(tour =>
                        new NamedChoice<Action>(
                            Translation.GetReplacement("employeeTourOption", new() { tour.Start.ToShortTimeString(), TourService.GetRemainingCapacity(tour).ToString() }),
                            () => { ShowTourMenu(tour); }))
                    .ToList());

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        private void ShowTourMenu(Tour tour)
        {
            CurrentMenu = MenuLevel.ActionsMenu;

            while (IsRunning && CurrentMenu >= MenuLevel.ActionsMenu)
            {
                AnsiConsole.Clear();
                Print("tourMenu", new() { tour.Start.ToShortTimeString(), TourService.GetRemainingCapacity(tour).ToString() });

                var options = new List<NamedChoice<Action>>();
                if (!tour.Departed)
                {
                    options.Add(new NamedChoice<Action>($"{Translation.Get("startTour")}", () => { BeginStartTour(tour); }));
                    if (tour.Participants.Count < tour.Capacity)
                        options.Add(new NamedChoice<Action>($"{Translation.Get("addGuest")}", () => { BeginAddGuest(tour); }));
                    if (tour.Participants.Count > 0)
                        options.Add(new NamedChoice<Action>($"{Translation.Get("removeGuest")}", () => { BeginRemoveGuest(tour); }));
                }
                options.Add(new NamedChoice<Action>($"{Translation.Get("return")}", () => { ExitToMenu(MenuLevel.SubMenu); }));

                Prompts.ShowMenu("chooseOption", options).Invoke();
            }
        }

        private void BeginStartTour(Tour tour)
        {
            var flow = GetFlow<StartTourFlow>();

            Print("employeeTourInfo", new() { tour.Start.ToShortTimeString() });
            Console.WriteLine();

            Print("registeredGuests");
            foreach (var guestNumber in tour.Participants)
                Print("guestList", new() { guestNumber });
            Console.WriteLine();

            if (!HandleFlowResult(flow.SetTour(tour)))
                return;

            if (tour.EmployeeNumber != Employee!.EmployeeNumber)
            {
                if (!string.IsNullOrWhiteSpace(tour.EmployeeNumber))
                {
                    Print("notYourTour");
                    if (!Prompts.AskYesNo("confirmYesNo", "yes", "no"))
                    {
                        Prompts.ShowSpinner("returningToMenu", 2000);
                        return;
                    }
                }

                if (!HandleFlowResult(flow.SetEmployeeNumber(Employee.EmployeeNumber)))
                    return;
            }

            Print("scanAllTickets");

            flow.Scanning = true;
            while (flow.ScannedTickets.Count != tour.Participants.Count && flow.Scanning)
            {
                if (!int.TryParse(Prompts.AskTicketOrEmployeeNumber("ticketNumberOrEmployeeNumber"), out int number))
                    continue;

                if (number < 10000000)
                {
                    if (!EmployeeService.ValidateEmployeeNumber(number.ToString()))
                    {
                        Print("invalidEmployeeNumber");
                        continue;
                    }

                    Print("finishedScanningQuestion");
                    if (Prompts.AskYesNo("confirmYesNo", "yes", "no"))
                    {
                        flow.Scanning = false;
                        continue;
                    }
                }

                if (!HandleFlowResult(flow.AddGuest(number.ToString())))
                    return;

                Print("ticketScanned", new() { number.ToString() });
                Affirmation();
            }

            Print("finishedScanning");

            flow.Scanning = true;
            while (flow.ScannedTickets.Count < tour.Capacity && flow.Scanning)
            {
                if (!int.TryParse(Prompts.AskTicketOrEmployeeNumber("ticketNumberOrEmployeeNumber"), out int number))
                    continue;

                if (number < 10000000)
                {
                    Print("finishedScanningQuestion");
                    if (Prompts.AskYesNo("confirmYesNo", "yes", "no"))
                    {
                        flow.Scanning = false;
                        continue;
                    }
                }

                if (!HandleFlowResult(flow.AddGuest(number.ToString(), true)))
                    return;

                Print("extraTicketScanned", new() { number.ToString() });
                Affirmation();
            }

            Prompts.ShowSpinner("finishedScanningExtraTickets", 2000);
            Console.Clear();

            Print("employeeTourInfo", new() { tour.Start.ToShortTimeString() });
            Console.WriteLine();

            Print("registeredGuests");
            foreach (var guestNumber in flow.ScannedTickets)
                Print("guestList", new() { guestNumber });

            HandleFlowConfirmation(flow,
                TGet("confirmStartTour"),
                TGet("startTourSuccess"),
                TGet("startTourCancelled"));

            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void BeginAddGuest(Tour tour)
        {
            var flow = GetFlow<AddGuestFlow>();

            Print("employeeTourInfo", new() { tour.Start.ToShortTimeString() });
            Console.WriteLine();

            if (!HandleFlowResult(flow.SetTour(tour)))
                return;

            if (!HandleFlowResult(flow.SetGuest(Prompts.AskTicketNumber("scanGuestTicket"))))
                return;

            HandleFlowConfirmation(flow,
                TGet("confirmAddGuest", new() { flow.Guest!.TicketNumber }),
                TGet("addSuccess", new() { flow.Guest!.TicketNumber }),
                TGet("addCancelled"));

            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void BeginRemoveGuest(Tour tour)
        {
            var flow = GetFlow<RemoveGuestFlow>();

            Print("employeeTourInfo", new() { tour.Start.ToShortTimeString() });
            Console.WriteLine();

            Print("registeredGuests");
            foreach (var guestNumber in tour.Participants)
                Print("guestList", new() { guestNumber });
            Console.WriteLine();

            if (!HandleFlowResult(flow.SetTour(tour)))
                return;

            if (!HandleFlowResult(flow.SetGuest(Prompts.AskTicketNumber("scanGuestTicket"))))
                return;

            HandleFlowConfirmation(flow,
                TGet("confirmGuestRemoval", new() { flow.Guest!.TicketNumber }),
                TGet("removeSuccess", new() { flow.Guest!.TicketNumber }),
                TGet("removeCancelled"));

            Prompts.ShowSpinner("returningToMenu", 2000);
        }
        #endregion

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
                    new NamedChoice<Action>($"{Translation.Get("createGuest")}", ShowCreateGuest),
                    new NamedChoice<Action>($"{Translation.Get("createEmployee")}", ShowCreateEmployee),
                    new NamedChoice<Action>($"{Translation.Get("createTours")}", ShowCreateTours),
                    new NamedChoice<Action>($"{Translation.Get("createBulkTickets")}", ShowCreateBulkTickets),
                    new NamedChoice<Action>($"{Translation.Get("logout")}", () => { ExitToMenu(MenuLevel.MainMenu); })
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

            Print("changesSaved", new() { changes.ToString() });
            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void ShowCreateTours()
        {
            int tourDuration = 40;

            DateOnly startDate = Prompts.AskDate("askStartDate");
            DateOnly endDate = Prompts.AskDate("askEndDate");
            if (startDate > endDate)
            {
                Print("startAfterEndDate");
                return;
            }

            TimeOnly startTime = Prompts.AskTime("askStartTime");
            TimeOnly endTime = Prompts.AskTime("askEndTime");
            if (startTime > endTime)
            {
                Print("startAfterEndTime");
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

                Print("changesSaved", new() { changes.ToString() });
            }
            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void ShowCreateGuest()
        {
            string ticketNumber = Prompts.AskTicketNumber("ticketNumber");
            DateOnly validForDate = Prompts.AskDate("askValidityDate");

            GuestService.Add(new Guest() { TicketNumber = ticketNumber, ValidDate = validForDate });
            int changes = GuestService.SaveChanges();

            Print("changesSaved", new() { changes.ToString() });
            Prompts.ShowSpinner("returningToMenu", 2000);
        }

        private void ShowCreateEmployee()
        {
            string username = Prompts.AskUsername("username");
            string password = Prompts.AskPassword("password");
            string employeeNumber = Prompts.AskNumber("employeeNumber", 1, 9999999).ToString();
            UserRole role = Prompts.AskRole("chooseUserRole");

            // Create new employee
            EmployeeService.Add(new Employee() { Username = username, Role = role, Password = password, EmployeeNumber = employeeNumber });
            int changes = EmployeeService.SaveChanges();

            Print("changesSaved", new() { changes.ToString() });
            Prompts.ShowSpinner("returningToMenu", 2000);
        }
        #endregion

        #region Shared
        private void ExitToMenu(MenuLevel menuLevel = MenuLevel.MainMenu)
        {
            CurrentMenu = menuLevel;
        }

        private void Affirmation()
        {
            System.Console.Beep(4000, 500);
        }
        private bool HandleFlowResult((bool Success, string MessageKey) result)
        {
            if (!result.Success)
            {
                Print(result.MessageKey);
                Prompts.ShowSpinner("returningToMenu", 2000);
            }
            return result.Success;
        }

        public void HandleFlowConfirmation(AbstractWorkflow flow, string title, string responseTrue, string responseFalse)
        {
            Console.MarkupLine(title);
            if (Prompts.AskYesNo("confirmYesNo", "yes", "no"))
            {
                Console.MarkupLine(responseTrue);
                flow.Commit();
            }
            else
            {
                Console.MarkupLine(responseFalse);
            }
        }

        private bool HandleConfirmation()
        {
            if (!Prompts.AskYesNo("confirmYesNo", "yes", "no"))
            {
                Prompts.ShowSpinner("returningToMenu", 2000);
                return false;
            }
            return true;
        }

        private void Print(string key, List<string>? replacements = null)
        {
            Console.MarkupLine(TGet(key, replacements));
        }

        private string TGet(string key, List<string>? replacements = null)
        {
            return replacements == null ? Translation.Get(key) : Translation.GetReplacement(key, replacements);
        }

        private T GetFlow<T>() where T : AbstractWorkflow => ServiceProvider.GetService<T>()!;
        #endregion
    }
}
