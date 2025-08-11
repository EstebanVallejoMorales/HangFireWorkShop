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
        _logger.LogInformation($"[CreateReservation] inicio: {passengerName} flight:{flightId}");
        var reservation = new Reservation { FlightId = flightId, PassengerName = passengerName, Status = "Pending" };
        _db.Reservations.Add(reservation);
        _db.SaveChanges();
        Console.WriteLine($"[CreateReservation] Reserva creada Id={reservation.Id}");
        _logger.LogInformation($"[CreateReservation] creada Id={reservation.Id}");
    }

    public void ConfirmReservation(int reservationId)
    {
        Console.WriteLine($"[ConfirmReservation] intentando {reservationId}");
        var reservation = _db.Reservations.Find(reservationId);
        if (reservation == null) { Console.WriteLine($"[ConfirmReservation] Reservation {reservationId} no encontrada"); return; }
        reservation.Status = "Confirmed";
        _db.SaveChanges();
        Console.WriteLine($"[ConfirmReservation] Confirmada {reservationId}");
    }

    public void SendReminder(int reservationId)
    {
        Console.WriteLine($"[SendReminder] Recordatorio para reservation {reservationId}");
        // Simular notificación
    }

    public void GenerateDailySummary()
    {
        var count = _db.Reservations.Count();
        Console.WriteLine($"[GenerateDailySummary] Total reservas en BD: {count}");
    }

    public void ProcessPayment(int reservationId)
    {
        Console.WriteLine($"[ProcessPayment] Procesando pago {reservationId} (simulado)...");
        System.Threading.Thread.Sleep(2000); // simula trabajo
        var r = _db.Reservations.Find(reservationId);
        if (r == null) { Console.WriteLine($"[ProcessPayment] no existe {reservationId}"); return; }
        r.Status = "Paid";
        _db.SaveChanges();
        Console.WriteLine($"[ProcessPayment] Pago completado {reservationId}");
    }

    public void SendTicket(int reservationId)
    {
        Console.WriteLine($"[SendTicket] Enviando ticket {reservationId} (simulado)...");
        var r = _db.Reservations.Find(reservationId);
        if (r == null) { Console.WriteLine($"[SendTicket] no existe {reservationId}"); return; }
        r.Status = "TicketSent";
        _db.SaveChanges();
        Console.WriteLine($"[SendTicket] Ticket enviado {reservationId}");
    }
}
