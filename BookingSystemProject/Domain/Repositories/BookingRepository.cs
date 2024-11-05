using BookingSystemProject.Domain.Entities;
using BookingSystemProject.Domain.Interfaces;

namespace BookingSystemProject.Domain.Repositories;

public class BookingRepository : IBookingRepository
{
    private List<Booking> _bookings;

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
        _bookings.RemoveAll(b => b.BookingId == bookingId);
    }

    public Booking GetBookingById(int bookingId)
    {
        return _bookings.Find(b => b.BookingId == bookingId);
    }

    public List<Booking> GetAllBookings()
    {
        return _bookings;
    }


}
