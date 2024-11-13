using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Application.Interfaces;

public interface IValidationService
{
    bool IsValidDate(string enteredDate);

    bool IsEmptyList<T>(IEnumerable<T> list);

    bool IsValidSeatNumber(string seatNumber, IEnumerable<Seat> seats); // rather ID

    bool IsValidBookingId(string bookingId, IEnumerable<Booking> bookings);

    bool IsValidUser(string userId, IEnumerable<Employee> employees);

}
