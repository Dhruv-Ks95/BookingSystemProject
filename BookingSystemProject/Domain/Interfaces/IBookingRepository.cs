using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Domain.Interfaces;

public interface IBookingRepository
{
    void AddBooking(Booking booking); // return id
    void RemoveBooking(int bookingId); // return bool
    Booking GetBookingById(int bookingId);
    IEnumerable<Booking> GetAllBookings(); // Ienumerable

}
