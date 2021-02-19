using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinEventUI.Data.DataObjects
{
    public class Participant
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int AtendeeCount { get; set; }
    }
}
