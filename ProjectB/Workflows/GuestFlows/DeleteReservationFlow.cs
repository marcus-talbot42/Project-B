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
    public class DeleteReservationFlow(IDatabaseContext context, ITourService tourService) : AbstractWorkflow(context)
    {
        public Guest? Guest { get; private set; }
        public Tour? CurrentTour { get; private set; }

        public (bool Success, string MessageKey) SetGuest(Guest guest)
        {
            if (guest == null)
                return (false, "guestIsNull");

            var currentTour = tourService.GetTourForGuest(guest);
            if (currentTour == null)
                return (false, "noReservationFound");

            if (currentTour.Departed)
                return (false, "tourDeparted");

            Guest = guest;
            CurrentTour = currentTour;

            return (true, string.Empty);
        }

        public override (bool Success, string MessageKey) Commit()
        {
            if (Guest == null)
                return (false, "guestIsNull");

            if (CurrentTour == null)
                return (false, "tourIsNull");

            tourService.DeleteReservationGuest(Guest);

            //No need to call base commit for this due to the service handling the changes
            return (true, string.Empty);
        }
    }
}
