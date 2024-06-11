using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;

namespace ProjectB.Workflows.EmployeeFlows
{
    public class AddGuestFlow(IDatabaseContext context, ITourService tourService, IGuestService guestService) : AbstractWorkflow(context)
    {
        public Guest? Guest { get; private set; }
        public Tour? Tour { get; private set; }

        public (bool Success, string MessageKey) SetGuest(string ticketNumber)
        {
            if (string.IsNullOrWhiteSpace(ticketNumber))
                return (false, "guestIsNull");

            var guest = guestService.FindValidGuestById(ticketNumber);

            if (guest == null)
                return (false, "invalidTicket");

            if (tourService.GetTourForGuest(guest) != null)
                return (false, "alreadyHasReservation");

            Guest = guest;

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

        public override (bool Success, string MessageKey) Commit()
        {
            if (Guest == null)
                return (false, "guestIsNull");

            if (Tour == null)
                return (false, "tourIsNull");

            tourService.RegisterGuestForTour(Guest, Tour);

            //No need to call base commit for this due to the service handling the changes
            return (true, string.Empty);
        }
    }
}
