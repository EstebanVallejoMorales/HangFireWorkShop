using HangFireWorkShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireWorkShop.Data;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.Flights.Any())
        {
            var flight = new Flight
            {
                FlightNumber = "AV123",
                Origin = "BOG",
                Destination = "MDE",
                Departure = DateTime.UtcNow.AddHours(3)
            };

            context.Flights.Add(flight);
            context.SaveChanges();

            Console.WriteLine("[DbInitializer] Vuelo de prueba creado con Id=" + flight.Id);
        }
        else
        {
            Console.WriteLine("[DbInitializer] Ya existen vuelos, no se crean nuevos.");
        }
    }
}

