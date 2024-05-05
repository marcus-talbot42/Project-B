using System;
using System.Collections.Generic;

namespace ProjectB.Models
{
    public class TourSignUp
    {
        public ICollection<Tour> SignUps { get; set; }

        public TourSignUp(ICollection<Tour> signUps)
        {
            SignUps = signUps;
        }

        public List<string> GetParticipants(DateTime tourTime)
        {
            List<string> participants = new List<string>();
            foreach (var tour in SignUps)
            
            {
            
                foreach (var participant in tour.GetParticipants())
                {
                    participants.Add(participant.GetId());
                }
            }

            return participants;
        }
    }
}
