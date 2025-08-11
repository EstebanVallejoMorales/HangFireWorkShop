using Hangfire;
using HangFireWorkShop.Api.Dto;
using HangFireWorkShop.Data;
using HangFireWorkShop.Domain;
using HangFireWorkShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace HangFireWorkShop.Api.Controllers;

[ApiController]
[Route("api/demo")]
public class DemoController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public DemoController(ApplicationDbContext db) { _db = db; }

    // 1) Fire-and-forget -> encola crear reserva en background
    [HttpPost("fireforget")]
    public IActionResult FireForget([FromBody] FireDto dto)
    {
        // Encolamos job. Se ejecutará lo antes posible.
        BackgroundJob.Enqueue<IReservationService>(svc => svc.CreateReservation(dto.FlightId, dto.PassengerName));
        return Ok("Fire-and-forget job enqueued");
    }

    // 2) Delayed job -> creamos la reserva y programamos confirmación después de X segundos
    [HttpPost("delayed")]
    public IActionResult Delayed([FromBody] DelayedDto dto)
    {
        var reservation = new Reservation { FlightId = dto.FlightId, PassengerName = dto.PassengerName, Status = "Pending" };
        _db.Reservations.Add(reservation);
        _db.SaveChanges();
        // Programamos confirmación en N segundos (para demo usa 20s)
        BackgroundJob.Schedule<IReservationService>(svc => svc.ConfirmReservation(reservation.Id), TimeSpan.FromSeconds(dto.DelaySeconds));
        return Ok(new { ReservationId = reservation.Id });
    }

    // 3) Continuation -> creamos la reserva, encolamos procesamiento de pago, y al terminar enviamos ticket
    [HttpPost("continuation")]
    public IActionResult Continuation([FromBody] FireDto dto)
    {
        var reservation = new Reservation 
        { 
            FlightId = dto.FlightId, 
            PassengerName = dto.PassengerName, 
            Status = "Pending" 
        };
        _db.Reservations.Add(reservation);
        _db.SaveChanges();

        // job principal: procesar pago
        var jobId = BackgroundJob.Enqueue<IReservationService>(svc => svc.ProcessPayment(reservation.Id));
        // continuación: enviar ticket cuando termine el pago
        BackgroundJob.ContinueJobWith<IReservationService>(jobId, svc => svc.SendTicket(reservation.Id));

        return Ok(new { ReservationId = reservation.Id, JobId = jobId });
    }

    // 4) (Recurring job no necesita endpoint; está en Program.cs)
}
