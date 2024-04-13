using ProjectB.Models;
using System;

namespace ProjectB.Repositories
{
    public class TourRepository : AbstractRepository<Tour, DateTime>
    {

        private static readonly Lazy<TourRepository> Lazy = new(() => new TourRepository());

        public static TourRepository Instance => Lazy.Value;
        
        private TourRepository() : base() {}

        public override string GetFileLocation() => "signups.json";

        public List<string> GetParticipants(DateTime tourTime) {
            var tour = FindById(tourTime);
            if (tour == null) {
                Console.WriteLine("No sign-up found for that DateTime.");
                return new List<string>();
            }
            return tour.Participants.Select(participant => participant.GetId()).ToList();
        }

    }
}
