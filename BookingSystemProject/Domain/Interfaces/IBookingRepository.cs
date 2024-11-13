using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Domain.Interfaces;

public interface IBookingRepository
{
    int AddBooking(Booking booking);
    bool RemoveBooking(int bookingId);
    Booking GetBookingById(int bookingId);
    IEnumerable<Booking> GetAllBookings(); 

}
