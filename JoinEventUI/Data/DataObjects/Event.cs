using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinEventUI.Data
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int ParticipantCount { get; set; }
        public int MaxParticipants { get; set; }
        public DateTime Date { get; set; }
        public string HTMLMessage { get; set; } = "";
        public string HTMLPage { get; set; } = "";
        public int PlacesLeft
        {
            get
            {
                return MaxParticipants - ParticipantCount;
            }
        }
    }
}
