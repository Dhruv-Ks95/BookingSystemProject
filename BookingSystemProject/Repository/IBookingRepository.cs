using BookingSystemProject.Models;
namespace BookingSystemProject.Repository;

public interface IBookingRepository
{
    void AddBooking(Booking booking);
    void RemoveBooking(int bookingId);
    Booking GetBookingById(int bookingId);
    List<Booking> GetAllBookings();
    
}
