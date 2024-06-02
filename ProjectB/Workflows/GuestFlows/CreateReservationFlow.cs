using ProjectB.Database;
using ProjectB.Models;
using ProjectB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB.Workflows.GuestFlows
{
    public class CreateReservationFlow(IDatabaseContext context, ITourService tourService) : AbstractWorkflow(context)
    {
        public Guest? Guest { get; private set; }
        public Tour? SelectedTour { get; private set; }

        public (bool Success, string MessageKey) SetGuest(Guest guest)
        {
            if (guest == null)
                return (false, "guestIsNull");

            if (tourService.GetTourForGuest(guest) != null)
                return (false, "alreadyHasReservation");

            Guest = guest;

            return (true, string.Empty);
        }

        public (bool Success, string MessageKey) SetTour(Tour tour)
        {
            if (tour == null)
                return (false, "tourIsNull");

            if (tour.Participants.Count > tour.Capacity)
                return (false, "tourFull");

            if (tour.Start < DateTime.Now)
                return (false, "tourInPast");

            if (tour.Departed)
                return (false, "tourDeparted");

            SelectedTour = tour;

            return (true, string.Empty);
        }

        public override (bool Success, string MessageKey) Commit()
        {
            if (Guest == null)
                return (false, "guestIsNull");

            if (SelectedTour == null)
                return (false, "tourIsNull");

            tourService.RegisterGuestForTour(Guest, SelectedTour);

            //No need to call base commit for this due to the service handling the changes
            return (true, string.Empty);
        }
    }
}
