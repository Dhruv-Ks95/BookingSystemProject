using BookingSystemProject.Application.Services;
using BookingSystemProject.Domain.Entities;
using BookingSystemProject.Domain.Repositories;
using FluentAssertions;

namespace BookingSystemProject.Tests;

public class BookingServiceTests
{
    private BookingService _bookingService;
    private SeatRepository _seatRepository;
    private BookingRepository _bookingRepository;

    public BookingServiceTests()
    {
        _bookingRepository = new BookingRepository();
        _seatRepository = new SeatRepository();
        _bookingService = new BookingService(_bookingRepository, _seatRepository);
    }

    // Admin adding bookings 
    [Fact]
    public void Admin_Should_Book_Seat_For_Employee()
    {
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        Employee user = new Employee("user", "user@company.com", RoleType.User);
        DateTime dateTime = DateTime.Today;
        int seatNumber = 1;

        _seatRepository.AddSeat(new Seat(seatNumber));

        var result = _bookingService.BookSeatForEmployee(admin, user, dateTime, seatNumber);
        result.Should().BeTrue();

        Booking addedBooking = _bookingRepository.GetAllBookings().Find(b => b.UserId == user.EmployeeId && b.BookingDate == dateTime);
        addedBooking.Should().NotBeNull();
        addedBooking.SeatNumber.Should().Be(seatNumber);
    }

    [Fact]
    public void Admin_Should_NotBook_InPast()
    {
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        Employee user = new Employee("user", "user@company.com", RoleType.User);
        DateTime dateTime = DateTime.Today.AddDays(-1);
        int seatNumber = 1;

        _seatRepository.AddSeat(new Seat(seatNumber));

        Action act = () => _bookingService.BookSeatForEmployee(admin,user, dateTime, seatNumber);

        act.Should().Throw<Exception>().WithMessage("Invalid Date Duration!");

    }

    [Fact]
    public void Admin_Should_Not_Book_AlreadyBooked_Seat()
    {
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        Employee user = new Employee("user", "user@company.com", RoleType.User);
        DateTime dateTime = DateTime.Today;
        int seatNumber = 1;

        _seatRepository.AddSeat(new Seat(seatNumber));
        _bookingRepository.AddBooking(new Booking(user.EmployeeId, 1, dateTime));
        Action act = () => _bookingService.BookSeatForEmployee(admin, user, dateTime, seatNumber);
        act.Should().Throw<Exception>().WithMessage("No seats available on provided Day!");
    }

    [Fact]
    public void NonAdmin_Should_Not_Be_Able_To_Book()
    {
        Employee frauduser = new Employee("frauduser", "fraud@company.com", RoleType.User);
        Employee user = new Employee("user", "user@company.com", RoleType.User);
        DateTime bookingDate = DateTime.Today;
        int seatNumber = 1;

        _seatRepository.AddSeat(new Seat(seatNumber));
        
        Action act = () => _bookingService.BookSeatForEmployee(frauduser,user, bookingDate, seatNumber);
        act.Should().Throw<Exception>().WithMessage("Unauthorized access to Admin feature!");
    }

    [Fact]
    public void Admin_Should_Not_Book_NonExistent_Seat()
    {
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        Employee user = new Employee("user", "user@company.com", RoleType.User);
        DateTime dateTime = DateTime.Today;
        int seatNumber = 99;
        _seatRepository.AddSeat(new Seat(1));

        Action act = () => _bookingService.BookSeatForEmployee(admin,user, dateTime, seatNumber);

        act.Should().Throw<Exception>().WithMessage("Invalid Seat Number Provided");
            
    }

    // Admin Viewing Bookings 
    [Fact]
    public void Admin_Should_Get_All_Bookings_On_Given_Date()
    {
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        Employee user1 = new Employee("user1", "user1@company.com", RoleType.User);
        Employee user2 = new Employee("user2", "user2@company.com", RoleType.User);
        var bookingDate = DateTime.Today;
        _seatRepository.AddSeat(new Seat(1));
        _seatRepository.AddSeat(new Seat(2));
        _bookingRepository.AddBooking(new Booking(user1.EmployeeId,1,bookingDate));
        _bookingRepository.AddBooking(new Booking(user2.EmployeeId,2,bookingDate));
        
        var result = _bookingService.GetAllBookingsOnDate(admin, bookingDate);

        result.Should().HaveCount(2);
        result.Should().Contain(b => b.SeatNumber == 1);
        result.Should().Contain(b => b.SeatNumber == 2);
    }

    [Fact]
    public void Admin_Should_Get_Empty_List_When_No_Bookings_Exist_On_Given_Date()
    {
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        DateTime bookingDate = DateTime.Today;

        var result = _bookingService.GetAllBookingsOnDate(admin, bookingDate);

        result.Should().BeEmpty();
    }

    [Fact]
    public void Admin_Should_Not_Retrieve_Bookings_On_Past_Date()
    {
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        DateTime pastDate = DateTime.Today.AddDays(-1);

        var result = _bookingService.GetAllBookingsOnDate(admin, pastDate);

        result.Should().BeEmpty(); 
    }


    // Admin Modifying bookings
    [Fact]
    public void Admin_Should_Successfully_Modify_Booking()
    {
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);
        DateTime bookingDate = DateTime.Today;
        int seatNumber = 1;
        _seatRepository.AddSeat(new Seat(1));
        _seatRepository.AddSeat(new Seat(2));

        var booking = new Booking(user.EmployeeId,seatNumber,bookingDate);
        _bookingRepository.AddBooking(booking);

        _bookingService.ModifyAnyBooking(admin, bookingDate, booking.BookingId, 2); 
        
        var modifiedBooking = _bookingRepository.GetBookingById(booking.BookingId);
        modifiedBooking.Should().NotBeNull();
        modifiedBooking.SeatNumber.Should().Be(2);
    }
    
    [Fact]
    public void Admin_Should_Not_Modify_Non_Existent_Booking()
    {
        
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        DateTime bookingDate = DateTime.Today;
        int invalidBookingId = 9;
        _bookingRepository.AddBooking(new Booking(1, 2, bookingDate));

        Action act = () => _bookingService.ModifyAnyBooking(admin, bookingDate, invalidBookingId, 2);

        act.Should().Throw<Exception>().WithMessage("Invalid Booking Id / Booking ID does not exist");
    }

    [Fact]
    public void Admin_Should_Not_Modify_Booking_To_Invalid_Seat_Number()
    {
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        DateTime bookingDate = DateTime.Today;
        int seatNumber = 1;
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);
        _seatRepository.AddSeat(new Seat(1));
        _seatRepository.AddSeat(new Seat(2));        

        Booking booking = new Booking(user.EmployeeId, seatNumber, bookingDate);
        _bookingRepository.AddBooking(booking);

        Action act = () => _bookingService.ModifyAnyBooking(admin, bookingDate, booking.BookingId, -1);
        act.Should().Throw<Exception>().WithMessage("Invalid seat number was provided");
    }

    [Fact]
    public void Admin_Should_Not_Modify_Booking_To_Past_Date()
    {
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        DateTime pastDate = DateTime.Today.AddDays(-1);
        int seatNumber = 1;
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);

        var booking = new Booking(user.EmployeeId,seatNumber, pastDate);
        _bookingRepository.AddBooking(booking);

        Action act = () => _bookingService.ModifyAnyBooking(admin, pastDate, booking.BookingId, 2);

        act.Should().Throw<Exception>().WithMessage("Incorrect DateTime");
    }

    // Admin Cancelling Bookings 

    [Fact]
    public void Admin_Should_Successfully_Cancel_Booking()
    {
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);
        DateTime bookingDate = DateTime.Today;
        int seatNumber = 1;

        var booking = new Booking(user.EmployeeId,seatNumber, bookingDate);
        _bookingRepository.AddBooking(booking);

        _bookingService.CancelAnyBooking(admin, booking.BookingId);

        var canceledBooking = _bookingRepository.GetBookingById(booking.BookingId);
        canceledBooking.Should().BeNull();
    }

    [Fact]
    public void Admin_Should_Not_Cancel_Non_Existent_Booking()
    {
        var admin = new Employee("Admin", "admin@company.com", RoleType.Admin);
        var invalidBookingId = 999;

        Action act = () => _bookingService.CancelAnyBooking(admin, invalidBookingId);

        act.Should().Throw<Exception>().WithMessage("Invalid Booking ID provided!");
    }

    [Fact]
    public void Non_Admin_Should_Not_Cancel_Booking()
    {
        Employee user = new Employee("frauduser","user@company.com",RoleType.User);
        DateTime bookingDate = DateTime.Today;
        int seatNumber = 1;

        Booking booking = new Booking(user.EmployeeId,seatNumber, bookingDate);
        _bookingRepository.AddBooking(booking);

        Action act = () => _bookingService.CancelAnyBooking(user, booking.BookingId);

        act.Should().Throw<Exception>().WithMessage("Unauthorized Access to admin feature!");
    }

    // User Adding Bookings
    [Fact]
    public void User_Should_Not_Book_If_No_Seats_Available_On_Given_Day()
    {
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);
        DateTime bookingDate = DateTime.Today;

        for (int i = 1; i <= 10; i++)
        {
            _bookingRepository.AddBooking(new Booking(user.EmployeeId,i,bookingDate));
        }

        Action act =() =>  _bookingService.BookSeat(user, bookingDate, 11);

        act.Should().Throw<Exception>().WithMessage("No seats available on provided Day!");
    }

    [Fact]
    public void User_Should_Not_Book_On_Past_Date()
    {
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);
        DateTime pastDate = DateTime.Today.AddDays(-1);
        int seatNumber = 5;

        Action act = () => _bookingService.BookSeat(user, pastDate, seatNumber);

        act.Should().Throw<Exception>().WithMessage("Invalid Date Duration!");
    }

    [Fact]
    public void User_Should_Not_Book_With_Invalid_Seat_Number()
    {
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);
        DateTime bookingDate = DateTime.Today;
        _seatRepository.AddSeat(new Seat(1));
        int invalidSeatNumber = -1;

        Action act = () => _bookingService.BookSeat(user, bookingDate, invalidSeatNumber);

        act.Should().Throw<Exception>().WithMessage("Invalid Seat Number Provided");
    }

    // Viewing User Bookings 
    [Fact]
    public void GetUserBookings_Should_Return_Empty_List_If_User_Has_No_Bookings()
    {
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);

        var bookings = _bookingService.GetUserBookings(user);

        bookings.Should().BeEmpty();
    }

    [Fact]
    public void GetUserBookings_Should_Return_All_Bookings_For_User()
    {
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);
        _seatRepository.AddSeat(new Seat(1));
        _seatRepository.AddSeat(new Seat(2));
        _seatRepository.AddSeat(new Seat(3));
        Booking booking1 = new Booking(user.EmployeeId, 1, DateTime.Today);
        Booking booking2 = new Booking(user.EmployeeId,2, DateTime.Today);

        _bookingRepository.AddBooking(booking1);
        _bookingRepository.AddBooking(booking2);

        var bookings = _bookingService.GetUserBookings(user);

        bookings.Should().HaveCount(2);
        bookings.Should().Contain(booking1);
        bookings.Should().Contain(booking2);
    }

    [Fact]
    public void GetUserBookings_Should_Not_Return_Other_Users_Bookings()
    {
        Employee user1 = new Employee("User1", "user1@company.com", RoleType.User);
        Employee user2 = new Employee("User2", "user2@company.com", RoleType.User);

        Booking bookingForUser2 = new Booking(user2.EmployeeId, 1, DateTime.Today);
        _bookingRepository.AddBooking(bookingForUser2);

        var bookings = _bookingService.GetUserBookings(user1);

        bookings.Should().BeEmpty();
    }

    // User cancelling bookings 
    [Fact]
    public void CancelUserBookings_Should_Return_False_If_User_Has_No_Bookings()
    {
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);
        int nonExistentBookingId = 1;

        Action act = () => _bookingService.CancelUserBookings(user, nonExistentBookingId);

        act.Should().Throw<Exception>().WithMessage("No User Bookings Exist inorder to Delete!");
    }

    [Fact]
    public void CancelUserBookings_Should_Cancel_Booking_With_Valid_BookingId()
    {
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);
        Booking booking = new Booking(user.EmployeeId, 1, DateTime.Today);

        _bookingRepository.AddBooking(booking);

        var result = _bookingService.CancelUserBookings(user, booking.BookingId);

        
        result.Should().BeTrue(); 
        _bookingRepository.GetAllBookings().Should().NotContain(booking); 
    }

    [Fact]
    public void CancelUserBookings_Should_Return_False_For_Invalid_BookingId()
    {
        Employee user = new Employee("User1", "user1@company.com", RoleType.User);
        Booking booking = new Booking(user.EmployeeId,1, DateTime.Today);
        
        _bookingRepository.AddBooking(booking);

        int invalidBookingId = 999;

        var result = _bookingService.CancelUserBookings(user, invalidBookingId);

        result.Should().BeFalse();
        _bookingRepository.GetAllBookings().Should().Contain(booking);
    }    

}