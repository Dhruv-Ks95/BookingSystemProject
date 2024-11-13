using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Application.Interfaces;

public interface IBookingService
{
    // For user services 
    int BookSeat(int empId, DateTime bookingDate, int SeatNumber);
    IEnumerable<Booking> GetUserBookings(int empId);
    bool CancelUserBookings(int empId, int bookingIdToDelete); 

    // For admin services
    int BookSeatForEmployee(int adminId, int empId, DateTime dateToBookOn, int seatToBook); 
    IEnumerable<Booking> GetAllBookingsOnDate(int adminId, DateTime dateToSearch);
    int ModifyAnyBooking(int adminId, DateTime dateToModifyBookingOn, int bookingId, int modifiedSeatNumber); // return id
    bool CancelAnyBooking(int adminId, int bookingId); // return bool

    // Additional Service
    IEnumerable<Seat> GetSeatsOnGivenDay(DateTime givenDate);

}
