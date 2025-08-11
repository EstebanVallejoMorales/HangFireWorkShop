using HangFireWorkShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HangFireWorkShop.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Flight> Flights { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:DefaultConnection"], options =>
            {
                options.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
            });
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Flight>().ToTable("Flights");
        modelBuilder.Entity<Reservation>().ToTable("Reservations");

        // Relationships

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Flight)
            .WithMany(f => f.Reservations)
            .HasForeignKey(r => r.FlightId);
    }
}
