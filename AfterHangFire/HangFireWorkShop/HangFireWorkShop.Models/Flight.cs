namespace HangFireWorkShop.Models;

public class Flight
{
    public int Id { get; set; }
    public string FlightNumber { get; set; }           // ej. "AV123"
    public string Origin { get; set; }                // ej. "BOG"
    public string Destination { get; set; }           // ej. "MDE"
    public DateTime Departure { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}