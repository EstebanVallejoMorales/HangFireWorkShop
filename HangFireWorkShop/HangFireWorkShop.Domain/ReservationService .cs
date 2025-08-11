using HangFireWorkShop.Data;

namespace HangFireWorkShop.Domain;

using HangFireWorkShop.Models;
using Microsoft.Extensions.Logging;

public class ReservationService : IReservationService
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(ApplicationDbContext db, ILogger<ReservationService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public void CreateReservation(int flightId, string passengerName)
    {
        _logger.LogInformation($"[CreateReservation] Starting: PassengerName: {passengerName}, Flight:{flightId}");
        var reservation = new Reservation { FlightId = flightId, PassengerName = passengerName, Status = "Pending" };
        _db.Reservations.Add(reservation);
        _db.SaveChanges();
        Console.WriteLine($"[CreateReservation] Reservation created: Id={reservation.Id}");
        _logger.LogInformation($"[CreateReservation] Created Id={reservation.Id}");
    }

    public void ConfirmReservation(int reservationId)
    {
        Console.WriteLine($"[ConfirmReservation] Trying {reservationId}");
        var reservation = _db.Reservations.Find(reservationId);
        if (reservation == null) { Console.WriteLine($"[ConfirmReservation] Reservation {reservationId} not found"); return; }
        reservation.Status = "Confirmed";
        _db.SaveChanges();
        Console.WriteLine($"[ConfirmReservation] Confirmed {reservationId}");
    }

    public void SendReminder(int reservationId)
    {
        // Simulate notification
        Console.WriteLine($"[SendReminder] Reservation Reminder {reservationId}");
    }

    public void GenerateDailySummary()
    {
        var count = _db.Reservations.Count();
        Console.WriteLine($"[GenerateDailySummary] Total reservations in DB: {count}");
    }

    public void ProcessPayment(int reservationId)
    {
        Console.WriteLine($"[ProcessPayment] Processing payment {reservationId} (simulated)...");
        System.Threading.Thread.Sleep(2000); // simulates work
        var reservation = _db.Reservations.Find(reservationId);
        if (reservation == null) { Console.WriteLine($"[ProcessPayment] does not exist {reservationId}"); return; }
        reservation.Status = "Paid";
        _db.SaveChanges();
        Console.WriteLine($"[ProcessPayment] Payment completed {reservationId}");
    }

    public void SendTicket(int reservationId)
    {
        Console.WriteLine($"[SendTicket] Sending ticket {reservationId} (simulated)...");
        var reservation = _db.Reservations.Find(reservationId);
        if (reservation == null) { Console.WriteLine($"[SendTicket] does not exist {reservationId}"); return; }
        reservation.Status = "TicketSent";
        _db.SaveChanges();
        Console.WriteLine($"[SendTicket] Ticket sent {reservationId}");
    }
}
