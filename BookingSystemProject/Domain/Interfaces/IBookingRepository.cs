using BookingSystemProject.Domain.Entities;

namespace BookingSystemProject.Domain.Interfaces;

public interface IBookingRepository
{
    void AddBooking(Booking booking);
    void RemoveBooking(int bookingId);
    Booking GetBookingById(int bookingId);
    List<Booking> GetAllBookings();

}
