using BookingSystemProject.Models;

namespace BookingSystemProject.Services;

internal interface IBookingService
{
    // For user services 
    bool BookSeat(Employee employee, DateTime bookingDate, int SeatNumber);
    List<Booking> GetUserBookings(Employee employee);
    bool CancelUserBookings(Employee employee, int bookingIdToDelete);

    // For admin services
    bool BookSeatForEmployee(Employee admin, Employee employeeToBookFor, DateTime dateToBookOn, int seatToBook);
    List<Booking> GetAllBookings(Employee admin, DateTime dateToSearch);
    void ModifyAnyBooking(Employee admin, DateTime dateToModifyBookingOn, int bookingId, int modifiedSeatNumber);
    void CancelAnyBooking(Employee admin, int bookingId);

    // Additional Service
    List<Seat> GetSeatsOnGivenDay(DateTime givenDate);

}
