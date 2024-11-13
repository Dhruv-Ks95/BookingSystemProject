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

    public void AddBooking(Booking booking)
    {
        _bookings.Add(booking);
    }

    public void RemoveBooking(int bookingId)
    {
        _bookings.RemoveAll(b => b.BookingId == bookingId); // RemoveAll to remove
    }

    public Booking GetBookingById(int bookingId)
    {
        return _bookings.Find(b => b.BookingId == bookingId);
    }

    public IEnumerable<Booking> GetAllBookings()
    {
        return _bookings;
    }


}
