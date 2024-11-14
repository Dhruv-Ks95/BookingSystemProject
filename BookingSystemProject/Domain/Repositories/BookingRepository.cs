using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Interfaces;

namespace Agdata.SeatBookingSystem.Domain.Repositories;

public class BookingRepository : IBookingRepository
{
    private List<Booking> _bookings; // For storing the bookings Data 

    public BookingRepository()
    {
        _bookings = new List<Booking>();
    }

    public int AddBooking(Booking booking)
    {
        _bookings.Add(booking);
        return booking.BookingId;
    }


    public Booking GetBookingById(int bookingId)
    {
        return _bookings.Find(b => b.BookingId == bookingId);
    }

    public bool RemoveBooking(int bookingId)
    {
        Booking bookingToDelete = GetBookingById(bookingId);
        _bookings.Remove(bookingToDelete);
        return true;
    }

    public IEnumerable<Booking> GetAllBookings()
    {
        return _bookings;
    }


}
