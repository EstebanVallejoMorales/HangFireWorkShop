using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireWorkShop.Domain
{
    public interface IReservationService
    {
        void CreateReservation(int flightId, string passengerName);
        void ConfirmReservation(int reservationId);
        void SendReminder(int reservationId);
        void GenerateDailySummary();
        void ProcessPayment(int reservationId);
        void SendTicket(int reservationId);
    }
}
