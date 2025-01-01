using Agdata.SeatBookingSystem.Application.Services;
using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Repositories;
using Agdata.SeatBookingSystem.Infrastructure.Data;
using Agdata.SeatBookingSystem.Tests.Mocks;
using FluentAssertions;

namespace Agdata.SeatBookingSystem.Tests;

public class BookingServiceTests
{
    SeatBookingDbContext dbContext = MockDbContext.GetDbContextWithTestData();

    private SeatRepository _seatRepository;
    private BookingRepository _bookingRepository;
    private EmployeeRepository _employeeRepository;

    private BookingService _bookingService;
    private EmployeeService _employeeService;

    public BookingServiceTests()
    {
        _bookingRepository = new BookingRepository(dbContext);
        _seatRepository = new SeatRepository(dbContext);
        _employeeRepository = new EmployeeRepository(dbContext);
        _employeeService = new EmployeeService(_employeeRepository);
        _bookingService = new BookingService(_bookingRepository, _seatRepository, _employeeService);
    }

    // Admin adding bookings 
    [Fact]
    public void Admin_Should_Book_Seat_For_Employee()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime dateTime = DateTime.Today.Date.AddDays(2);
        int seatNumber = 1;        
        int result = _bookingService.BookSeatForEmployee(admin.EmployeeId, user.EmployeeId, dateTime, seatNumber);
        result.Should().BeGreaterThan(0);

        Booking addedBooking = _bookingRepository.GetBookingById(result);
        addedBooking.Should().NotBeNull();
        addedBooking.SeatId.Should().Be(seatNumber);
    }

    [Fact]
    public void Admin_Should_NotBook_InPast()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime dateTime = DateTime.Today.Date.AddDays(-1);
        int seatNumber = 1;

        Action act = () => _bookingService.BookSeatForEmployee(admin.EmployeeId, user.EmployeeId, dateTime, seatNumber);

        act.Should().Throw<Exception>().WithMessage("Invalid Date Duration!");

    }

    [Fact]
    public void Admin_Should_Not_Book_AlreadyBooked_Seat()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime dateTime = DateTime.Today.Date.AddDays(17);
        int seatNumber = 1;
        
        _bookingRepository.AddBooking(new Booking(user.EmployeeId, dateTime,1));
        Action act = () => _bookingService.BookSeatForEmployee(admin.EmployeeId, user.EmployeeId, dateTime, seatNumber);
        act.Should().Throw<Exception>().WithMessage("Invalid Seat Number Provided");
    }

    [Fact]
    public void NonAdmin_Should_Not_Be_Able_To_Book()
    {
        Employee frauduser = _employeeService.GetEmployeeByEmployeeId(2);
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime bookingDate = DateTime.Today.Date;
        int seatNumber = 1;

        Action act = () => _bookingService.BookSeatForEmployee(frauduser.EmployeeId, user.EmployeeId, bookingDate, seatNumber);
        act.Should().Throw<Exception>().WithMessage("Unauthorized access to Admin feature!");
    }

    [Fact]
    public void Admin_Should_Not_Book_NonExistent_Seat()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime dateTime = DateTime.Today.Date;
        int seatNumber = 99;        

        Action act = () => _bookingService.BookSeatForEmployee(admin.EmployeeId, user.EmployeeId, dateTime, seatNumber);

        act.Should().Throw<Exception>().WithMessage("Invalid Seat Number Provided");

    }

    // Admin Viewing Bookings 
    [Fact]
    public void Admin_Should_Get_All_Bookings_On_Given_Date()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        var bookingDate = DateTime.Today.Date.AddDays(12);       
        _bookingRepository.AddBooking(new Booking(user.EmployeeId, bookingDate,5));
        _bookingRepository.AddBooking(new Booking(user.EmployeeId, bookingDate,6));

        var result = _bookingService.GetAllBookingsOnDate(admin.EmployeeId, bookingDate);

        result.Should().HaveCount(2);
        result.Should().Contain(b => b.SeatId == 5);
        result.Should().Contain(b => b.SeatId == 6);
    }

    [Fact]
    public void Admin_Should_Get_Empty_List_When_No_Bookings_Exist_On_Given_Date()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        DateTime bookingDate = DateTime.Today.Date.AddDays(7);

        var result = _bookingService.GetAllBookingsOnDate(admin.EmployeeId, bookingDate);

        result.Should().BeEmpty();
    }

    [Fact]
    public void Admin_Should_Not_Retrieve_Bookings_On_Past_Date()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        DateTime pastDate = DateTime.Today.Date.AddDays(-2);

        var result = _bookingService.GetAllBookingsOnDate(admin.EmployeeId, pastDate);

        result.Should().BeEmpty();
    }


    // Admin Modifying bookings
    [Fact]
    public void Admin_Should_Successfully_Modify_Booking()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime bookingDate = DateTime.Today.Date.AddDays(8);
        int seatId = 1;

        var booking = new Booking(user.EmployeeId, bookingDate, seatId);
        _bookingRepository.AddBooking(booking);

        _bookingService.ModifyAnyBooking(admin.EmployeeId, bookingDate, booking.BookingId, 2);

        var modifiedBooking = _bookingRepository.GetBookingById(booking.BookingId);
        modifiedBooking.Should().NotBeNull();
        modifiedBooking.SeatId.Should().Be(2);
    }

    [Fact]
    public void Admin_Should_Not_Modify_Non_Existent_Booking()
    {

        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime bookingDate = DateTime.Today.Date.AddDays(9);
        int invalidBookingId = 999;
        int seatId = 4;
        _bookingRepository.AddBooking(new Booking(user.EmployeeId, bookingDate,seatId));        
        Action act = () => _bookingService.ModifyAnyBooking(admin.EmployeeId, bookingDate, invalidBookingId, 2);

        act.Should().Throw<Exception>().WithMessage("Invalid Booking Id / Booking ID does not exist");
    }

    [Fact]
    public void Admin_Should_Not_Modify_Booking_To_Invalid_Seat_Number()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime bookingDate = DateTime.Today.Date;
        int seatId = 1;

        Booking booking = new Booking(user.EmployeeId,bookingDate, seatId);
        _bookingRepository.AddBooking(booking);

        Action act = () => _bookingService.ModifyAnyBooking(admin.EmployeeId, bookingDate, booking.BookingId, -1);
        act.Should().Throw<Exception>().WithMessage("Invalid seat number was provided");
    }

    [Fact]
    public void Admin_Should_Not_Modify_Booking_To_Past_Date()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime pastDate = DateTime.Today.Date.AddDays(-1);
        int seatId = 1;

        var booking = new Booking(user.EmployeeId, pastDate, seatId);
        _bookingRepository.AddBooking(booking);

        Action act = () => _bookingService.ModifyAnyBooking(admin.EmployeeId, pastDate, booking.BookingId, 2);

        act.Should().Throw<Exception>().WithMessage("Incorrect DateTime");
    }

    // Admin Cancelling Bookings 

    [Fact]
    public void Admin_Should_Successfully_Cancel_Booking()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime bookingDate = DateTime.Today.Date;
        int seatId = 1;

        var booking = new Booking(user.EmployeeId, bookingDate, seatId);
        _bookingRepository.AddBooking(booking);

        _bookingService.CancelAnyBooking(admin.EmployeeId, booking.BookingId);

        var canceledBooking = _bookingRepository.GetBookingById(booking.BookingId);
        canceledBooking.Should().BeNull();
    }

    [Fact]
    public void Admin_Should_Not_Cancel_Non_Existent_Booking()
    {
        Employee admin = _employeeService.GetEmployeeByEmployeeId(1);
        int invalidBookingId = 999;

        var result = _bookingService.CancelAnyBooking(admin.EmployeeId, invalidBookingId);

        result.Should().BeFalse();
    }

    [Fact]
    public void Non_Admin_Should_Not_Cancel_Booking()
    {
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime bookingDate = DateTime.Today.Date;
        int seatId = 1;

        Booking booking = new Booking(user.EmployeeId, bookingDate, seatId);
        _bookingRepository.AddBooking(booking);

        Action act = () => _bookingService.CancelAnyBooking(user.EmployeeId, booking.BookingId);

        act.Should().Throw<Exception>().WithMessage("Unauthorized Access to admin feature!");
    }

    // User Adding Bookings
    [Fact]
    public void User_Should_Not_Book_If_No_Seats_Available_On_Given_Day()
    {
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime bookingDate = DateTime.Today.Date.AddDays(15);

        for (int i = 1; i <= 10; i++)
        {
            _bookingRepository.AddBooking(new Booking(user.EmployeeId, bookingDate, i));
        }

        Action act = () => _bookingService.BookSeat(user.EmployeeId, bookingDate, 11);

        act.Should().Throw<Exception>().WithMessage("No seats available on provided Day!");
    }

    [Fact]
    public void User_Should_Not_Book_On_Past_Date()
    {
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime pastDate = DateTime.Today.Date.AddDays(-1);
        int seatId= 5;

        Action act = () => _bookingService.BookSeat(user.EmployeeId, pastDate, seatId);

        act.Should().Throw<Exception>().WithMessage("Invalid Date Duration!");
    }

    [Fact]
    public void User_Should_Not_Book_With_Invalid_Seat_Number()
    {
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        DateTime bookingDate = DateTime.Today.Date;        
        int invalidSeatNumber = -1;

        Action act = () => _bookingService.BookSeat(user.EmployeeId, bookingDate, invalidSeatNumber);

        act.Should().Throw<Exception>().WithMessage("Invalid Seat Number Provided");
    }

    // Viewing User Bookings 
    [Fact]
    public void GetUserBookings_Should_Return_Empty_List_If_User_Has_No_Bookings()
    {
        Employee user = _employeeService.GetEmployeeByEmployeeId(5);

        var bookings = _bookingService.GetUserBookings(user.EmployeeId);

        bookings.Should().BeEmpty();
    }

    [Fact]
    public void GetUserBookings_Should_Return_All_Bookings_For_User()
    {
        Employee user = _employeeService.GetEmployeeByEmployeeId(3);        
        Booking booking1 = new Booking(user.EmployeeId,DateTime.Today.Date, 2);
        Booking booking2 = new Booking(user.EmployeeId,DateTime.Today.Date, 3);

        _bookingRepository.AddBooking(booking1);
        _bookingRepository.AddBooking(booking2);

        var bookings = _bookingService.GetUserBookings(user.EmployeeId);

        bookings.Should().HaveCount(2);
        bookings.Should().Contain(booking1);
        bookings.Should().Contain(booking2);
    }

    [Fact]
    public void GetUserBookings_Should_Not_Return_Other_Users_Bookings()
    {
        Employee user1 = _employeeService.GetEmployeeByEmployeeId(3);
        Employee user2 = _employeeService.GetEmployeeByEmployeeId(2);

        Booking bookingForUser2 = new Booking(user2.EmployeeId, DateTime.Today.Date, 1);
        _bookingRepository.AddBooking(bookingForUser2);

        var bookings = _bookingService.GetUserBookings(user1.EmployeeId);

        bookings.Should().BeEmpty();
    }

    // User cancelling bookings 
    [Fact]
    public void CancelUserBookings_Should_Return_False_If_User_Has_No_Bookings()
    {
        Employee user = _employeeService.GetEmployeeByEmployeeId(3);
        int nonExistentBookingId = 1;

        Action act = () => _bookingService.CancelUserBookings(user.EmployeeId, nonExistentBookingId);

        act.Should().Throw<Exception>().WithMessage("No User Bookings Exist inorder to Delete!");
    }

    [Fact]
    public void CancelUserBookings_Should_Cancel_Booking_With_Valid_BookingId()
    {
        Employee user = _employeeService.GetEmployeeByEmployeeId(3);
        Booking booking = new Booking(user.EmployeeId, DateTime.Today.Date, 1 );

        _bookingRepository.AddBooking(booking);

        var result = _bookingService.CancelUserBookings(user.EmployeeId, booking.BookingId);


        result.Should().BeTrue();
        _bookingRepository.GetAllBookings().Should().NotContain(booking);
    }

    [Fact]
    public void CancelUserBookings_Should_Return_False_For_Invalid_BookingId()
    {
        Employee user = _employeeService.GetEmployeeByEmployeeId(2);
        Booking booking = new Booking(user.EmployeeId, DateTime.Today.Date, 1);

        _bookingRepository.AddBooking(booking);

        int invalidBookingId = 999;

        var result = _bookingService.CancelUserBookings(user.EmployeeId, invalidBookingId);

        result.Should().BeFalse();
        _bookingRepository.GetAllBookings().Should().Contain(booking);
    }    
}