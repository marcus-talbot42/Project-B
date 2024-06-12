using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectB.Workflows.EmployeeFlows
{
    public class StartTourFlow(IDatabaseContext context, ITourService tourService, IGuestService guestService, IEmployeeService employeeService) : AbstractWorkflow(context)
    {
        public Tour? Tour { get; private set; }
        public string EmployeeNumber { get; private set; }
        public List<string> ScannedTickets { get; private set; } = new List<string>();
        public bool Scanning { get; set; }

        public (bool Success, string MessageKey) AddGuest(string ticketNumber, bool extra = false)
        {
            if (string.IsNullOrWhiteSpace(ticketNumber))
                return (false, "guestIsNull");

            var guest = guestService.FindValidGuestById(ticketNumber);

            if (guest == null)
                return (false, "invalidTicket");

            if (ScannedTickets.Contains(ticketNumber))
                return (false, "ticketAlreadyScanned");

            if (Tour == null)
                return (false, "tourIsNull");

            if (!Tour.Participants.Contains(ticketNumber.ToString()) && !extra)
                return (false, "ticketNotInTour");

            ScannedTickets.Add(ticketNumber);

            return (true, string.Empty);
        }

        public (bool Success, string MessageKey) SetTour(Tour tour)
        {
            if (tour == null)
                return (false, "tourIsNull");

            if (tour.Participants.Count >= tour.Capacity)
                return (false, "tourFull");

            if (tour.Start < DateTime.Now)
                return (false, "tourInPast");

            if (tour.Departed)
                return (false, "tourDeparted");

            Tour = tour;

            return (true, string.Empty);
        }

        public (bool Success, string MessageKey) SetEmployeeNumber(string employeeNumber)
        {
            if (string.IsNullOrWhiteSpace(employeeNumber))
                return (false, "employeeNumberIsNull");

            if (!employeeService.ValidateEmployeeNumber(employeeNumber))
                return (false, "employeeNumberInvalid");

            EmployeeNumber = employeeNumber;

            return (true, string.Empty);
        }
        
        public override (bool Success, string MessageKey) Commit()
        {
            if (!ScannedTickets.Any())
                return (false, "noScannedTickets");

            if (Tour == null)
                return (false, "tourIsNull");

            Tour.Participants.AddRange(ScannedTickets);
            Tour.EmployeeNumber = EmployeeNumber;
            Tour.Departed = true;

            //No need to call base commit for this due to the service handling the changes
            return base.Commit();
        }
    }
}
