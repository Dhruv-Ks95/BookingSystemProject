using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Interfaces;
using Agdata.SeatBookingSystem.Infrastructure.Data;

namespace Agdata.SeatBookingSystem.Domain.Repositories;

public class BookingRepository : IBookingRepository
{
    private SeatBookingDbContext context;

    public BookingRepository(SeatBookingDbContext _context)
    {
        context = _context;
    }

    public int AddBooking(Booking booking)
    {
        context.Bookings.Add(booking);
        context.SaveChanges();
        return booking.BookingId;
    }


    public Booking GetBookingById(int bookingId)
    {
        Booking booking = context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
        return booking;
    }

    public bool RemoveBooking(int bookingId)
    {
        Booking bookingToDelete = GetBookingById(bookingId);
        if (bookingToDelete != null)
        {
            context.Bookings.Remove(bookingToDelete);
            var rowsAffected = context.SaveChanges();
            return rowsAffected> 0;
        }
        return false;
    }

    public IEnumerable<Booking> GetAllBookings()
    {
        return context.Bookings.ToList();
    }


}
