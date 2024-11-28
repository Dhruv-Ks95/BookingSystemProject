using Agdata.SeatBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agdata.SeatBookingSystem.Infrastructure.Data;

public class SeatBookingDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    public SeatBookingDbContext() { }

    public SeatBookingDbContext(DbContextOptions<SeatBookingDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=DhruvKs\\SQLEXPRESS;Initial Catalog=SeatBookingSystemDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
        }
        //optionsBuilder.UseSqlServer("Data Source=DhruvKs\\SQLEXPRESS;Initial Catalog=SeatBookingSystemDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"); 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // For employees
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Role).IsRequired();
        });
        // For Seats
        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(s => s.SeatId);
            entity.Property(s => s.SeatNumber).IsRequired();
            entity.HasIndex(s => s.SeatNumber).IsUnique();
        });
        // For Bookings
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.BookingId);

            entity.HasOne<Employee>()
                  .WithMany()
                  .HasForeignKey(b => b.EmployeeId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Seat>()
                  .WithMany()
                  .HasForeignKey(b => b.SeatId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(b => b.BookingDate).IsRequired();
        });
    }
}
