using BookingSystemProject.Models;
using BookingSystemProject.Repository;

namespace BookingSystemProject.Services;

public class BookingService : IBookingService
{
    private IBookingRepository bookingRepo;
    private ISeatRepository seatRepo;
    public BookingService(IBookingRepository bookingRepository, ISeatRepository seatRepository)
    {
        bookingRepo = bookingRepository;
        seatRepo = seatRepository;
    }

    // Regular Booking Services
    public bool BookSeat(Employee employee, DateTime bookingDate, int SeatNumber)
    {
        // Validating Date
        if (!IsValidDate(bookingDate))
        {
            throw new Exception("Invalid Date Duration!");
        }

        // Available seat pool -> get available seats on that day
        var availableSeats = GetSeatsOnGivenDay(bookingDate);

        if (availableSeats.Count == 0)
        {
            throw new Exception("No seats available on provided Day!");
        }
        
        // Get Seat from available Seats
        Seat selectedSeat = null;
        foreach (var seat in availableSeats)
        {
            if (seat.SeatNumber == SeatNumber)
            {
                selectedSeat = seat;
                break;
            }
        }

        // Add the Selected Seat in Booking -> Mark as booked
        if (selectedSeat != null)
        {
            var booking = new Booking(employee.EmployeeId, selectedSeat.SeatNumber, bookingDate);
            bookingRepo.AddBooking(booking);
            return true;
        }
        else
        {
            throw new Exception("All seats are booked !");
        }
    }
    public List<Booking> GetUserBookings(Employee employee)
    {
        List<Booking> userBookings = new List<Booking>();
        foreach (var booking in bookingRepo.GetAllBookings()) 
        {
            if (booking.UserId == employee.EmployeeId && booking.BookingDate >= DateTime.Today && booking.BookingDate <= DateTime.Today.AddDays(30))
            {
                userBookings.Add(booking);
            }
        }            
        return userBookings;
    }
    public bool CancelUserBookings(Employee employee, int bookingIdToDelete)
    {
        List<Booking> userBookings = GetUserBookings(employee);

        if (userBookings.Count == 0)
        {
            throw new Exception("No User Bookings Exist inorder to Delete!");
        }

        Booking bookingToCancel = null;
        foreach (var booking in userBookings)
        {
            if (booking.BookingId == bookingIdToDelete)
            {
                bookingToCancel = booking;
                break;
            }
        }
        if (bookingToCancel == null)
        {
            return false;
        }
        bookingRepo.RemoveBooking(bookingIdToDelete);
        return true;
    }

    // Admin Booking Services
    public bool BookSeatForEmployee(Employee admin, Employee employeeToBookFor, DateTime dateToBookOn, int seatToBook) 
    {
        if(admin.Role != RoleType.Admin)
        {
            throw new Exception("Unauthorized access to Admin feature!");
        }
        return BookSeat(employeeToBookFor, dateToBookOn, seatToBook);
    }       
    public List<Booking> GetAllBookingsOnDate(Employee admin, DateTime dateToSearch)
    {
        if (admin.Role != RoleType.Admin)
        {
            throw new Exception("Unauthorized access to Admin Feature!");
        }

        List<Booking> bookingsOnSpecifiedDate = new List<Booking>();
        foreach (var booking in bookingRepo.GetAllBookings())
        {
            if (booking.BookingDate == dateToSearch)
            {
                bookingsOnSpecifiedDate.Add(booking);
            }
        }
        return bookingsOnSpecifiedDate;
    }
    public void ModifyAnyBooking(Employee admin, DateTime dateToModifyBookingOn,int bookingId, int modifiedSeatNumber)
    {
        if(admin.Role != RoleType.Admin)
        {
            throw new Exception("Unauthorized access to admin feature!");
        }

        // get all bookings on the given date -> just to check if bookings exist on that day
        List<Booking> bookingsOnDate = GetAllBookingsOnDate(admin, dateToModifyBookingOn);
        if(bookingsOnDate.Count == 0) // if no bookings exist 
        {
            throw new Exception("No bookings found on Selected Date!");
        }
        // Get Booking via bookingId
        Booking bookingToModify = bookingRepo.GetBookingById(bookingId);
        if (bookingToModify == null)
        {
            throw new Exception("Invalid Booking Id / Booking ID does not exist");
        }

        // get available seats on the selected date
        List<Seat> availableSeats = GetSeatsOnGivenDay(dateToModifyBookingOn);
        if(availableSeats.Count == 0)
        {
            throw new Exception("No available seats left for modification!");
        }

        // make sure selected seat is available 
        bool isAvailable = false;
        foreach (var availableSeat in availableSeats)
        {
            if(modifiedSeatNumber == availableSeat.SeatNumber)
            {
                isAvailable = true;
            }
        }

        if (!isAvailable)
        {
            throw new Exception("Invalid seat number was provided");
        }

        // Now update the seatnumber 
        bookingToModify.SeatNumber = modifiedSeatNumber;      
    }    
    public void CancelAnyBooking(Employee admin, int bookingId)
    {
        if(admin.Role != RoleType.Admin)
        {
            throw new Exception("Unauthorized Access to admin feature!");
        }

        bool bookingExists = false;
        Booking bookingToDelete = null;
        foreach (var booking in bookingRepo.GetAllBookings())
        {
            if (booking.BookingId == bookingId)
            {
                bookingToDelete = booking;
                bookingExists = true;
                break;
            }
        }
        if (!bookingExists)
        {
            throw new Exception("Invalid Booking ID provided!");
        }
        bookingRepo.RemoveBooking(bookingId);
    }


    // Additional Methods

    public List<Seat> GetSeatsOnGivenDay(DateTime givenDate)
    {
        List<Seat> availableSeats = new List<Seat>();
        foreach (var seat in seatRepo.GetAllSeats())
        {
            bool isBooked = false;

            foreach (var booking in bookingRepo.GetAllBookings())
            {
                if (booking.SeatNumber == seat.SeatNumber && booking.BookingDate == givenDate)
                {
                    isBooked = true;
                    break;
                }
            }

            if (!isBooked)
            {
                availableSeats.Add(seat);
            }
        }
        return availableSeats;
    }
    private bool IsValidDate(DateTime givenDate)
    {
        if(givenDate < DateTime.Today ||  givenDate > DateTime.Today.AddDays(30))
        {
            return false;            
        }
        return true;
    }            
}
