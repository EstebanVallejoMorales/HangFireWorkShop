using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireWorkShop.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public Flight Flight { get; set; }
        public string PassengerName { get; set; }
        public string Status { get; set; }    // "Pending", "Confirmed", "Paid", "TicketSent"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
