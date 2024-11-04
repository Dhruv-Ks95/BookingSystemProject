using BookingSystemProject.Models;

namespace BookingSystemProject.Services;

public interface IValidationService
{
    bool IsValidDate(string enteredDate);

    bool IsEmptyList<T>(List<T> list);

    bool IsValidSeatNumber(string seatNumber,List<Seat> seats);

    bool IsValidBookingId(string bookingId,List<Booking> bookings);

    bool IsValidUser(string userId,List<Employee> employees);

}
