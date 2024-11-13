using Agdata.SeatBookingSystem.Application.Interfaces;
using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Application.Services;

public class ValidationService : IValidationService
{
    public bool IsValidDate(string enteredDate)
    {
        if (DateTime.TryParse(enteredDate, out DateTime date))
        {
            if (date < DateTime.Today || date > DateTime.Today.AddDays(30))
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public bool IsEmptyList<T>(IEnumerable<T> lst)
    {
        if (lst == null || lst.Count() == 0) return true;

        return false;
    }

    public bool IsValidSeatNumber(string seatNumber, IEnumerable<Seat> seats)
    {
        if (int.TryParse(seatNumber, out int seatNum))
        {
            foreach (var seat in seats)
            {
                if (seat.SeatNumber == seatNum)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsValidBookingId(string bookingId, IEnumerable<Booking> bookings)
    {
        if (int.TryParse(bookingId, out int id))
        {
            foreach (var booking in bookings)
            {
                if (booking.BookingId == id)
                {
                    return true;
                }
            }

        }
        return false;
    }

    public bool IsValidUser(string user, IEnumerable<Employee> employees)
    {
        if (int.TryParse(user, out int id))
        {
            foreach (Employee employee in employees)
            {
                if (employee.EmployeeId == id)
                {
                    return true;
                }
            }
        }
        return false;
    }

}
