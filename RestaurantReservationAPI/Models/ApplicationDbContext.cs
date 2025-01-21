using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Models;
using System;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Carregar a variável de ambiente
            var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found in environment variables.");
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    public virtual DbSet<Reservation> Reservations { get; set; }
}
