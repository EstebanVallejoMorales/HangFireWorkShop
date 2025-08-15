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

    // 1) Fire-and-forget -> Enqueue create reservation in background
    [HttpPost("fireforget")]
    public IActionResult FireForget([FromBody] FireDto dto)
    {
        // We queue the job. It will run as soon as possible.
        BackgroundJob.Enqueue<IReservationService>(svc => svc.CreateReservation(dto.FlightId, dto.PassengerName));
        return Ok("Fire-and-forget job enqueued");
    }

    // 2) Delayed job -> We create the reservation and schedule confirmation after X seconds
    [HttpPost("delayed")]
    public IActionResult Delayed([FromBody] DelayedDto dto)
    {
        var reservation = new Reservation { FlightId = dto.FlightId, PassengerName = dto.PassengerName, Status = "Pending" };
        _db.Reservations.Add(reservation);
        _db.SaveChanges();
        // We schedule confirmation in N seconds
        BackgroundJob.Schedule<IReservationService>(svc => svc.ConfirmReservation(reservation.Id), TimeSpan.FromSeconds(dto.DelaySeconds));
        return Ok(new { ReservationId = reservation.Id });
    }

    // 3) Continuation -> We create the reservation, queue payment processing, and when finished we send the ticket.
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

        // Main job: process payment
        var jobId = BackgroundJob.Enqueue<IReservationService>(svc => svc.ProcessPayment(reservation.Id));
        // Continued: Send ticket when payment is complete
        BackgroundJob.ContinueJobWith<IReservationService>(jobId, svc => svc.SendTicket(reservation.Id));

        return Ok(new { ReservationId = reservation.Id, JobId = jobId });
    }

    // 4) (Recurring job doesn't need an endpoint; it's in Program.cs)
}
