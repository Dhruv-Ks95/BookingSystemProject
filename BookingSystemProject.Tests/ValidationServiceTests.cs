using BookingSystemProject.Application.Services;
using BookingSystemProject.Domain.Entities;
using FluentAssertions;
namespace BookingSystemProject.Tests;
public class ValidationServiceTests
{
    private ValidationService _validationService;

    public ValidationServiceTests()
    {
        _validationService = new ValidationService();
    }

    [Theory]
    [InlineData("2024-11-05", true)]
    [InlineData("2024-10-01", false)]
    [InlineData("2025-01-01", false)]
    [InlineData("invalid-date", false)]
    public void IsValidDate_Should_Return_Correct_Result(string enteredDate, bool expected)
    {
        var result = _validationService.IsValidDate(enteredDate);

        result.Should().Be(expected);
    }

    [Fact]
    public void IsEmptyList_Should_Return_True_For_Empty_List()
    {
        var emptyList = new List<int>();

        var result = _validationService.IsEmptyList(emptyList);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsEmptyList_Should_Return_False_For_Non_Empty_List()
    {
        var nonEmptyList = new List<int> { 1, 2, 3 };

        var result = _validationService.IsEmptyList(nonEmptyList);

        result.Should().BeFalse();
    }

    [Fact]
    public void IsEmptyList_Should_Return_True_For_Null_List()
    {
        List<int> nullList = null;

        var result = _validationService.IsEmptyList(nullList);

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("101", true)]
    [InlineData("999", false)]
    [InlineData("invalid-seat", false)]
    public void IsValidSeatNumber_Should_Return_Correct_Result(string seatNumber, bool expected)
    {
        var seats = new List<Seat>
        {
            new Seat(101),
            new Seat(102),
            new Seat(103)
        };

        var result = _validationService.IsValidSeatNumber(seatNumber, seats);

        result.Should().Be(expected);
    }

    [Theory]
    //[InlineData("1", true)]
    [InlineData("999", false)]
    [InlineData("invalid-id", false)]
    public void IsValidBookingId_Should_Return_Correct_Result(string bookingId, bool expected)
    {
        var bookings = new List<Booking>
        {
            new Booking(1, 1,DateTime.Today),
            new Booking (2, 2, DateTime.Today),
            new Booking (3, 3, DateTime.Today)
        };

        var result = _validationService.IsValidBookingId(bookingId, bookings);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("999", false)]
    [InlineData("invalid-id", false)]
    public void IsValidUser_Should_Return_Correct_Result(string userId, bool expected)
    {
        var employees = new List<Employee>
        {
            new Employee("dhruv","dhruv@company.com",RoleType.User),
            new Employee("shukla","shukla@company.com",RoleType.User),
            new Employee("das","das@company.com",RoleType.User)
        };

        var result = _validationService.IsValidUser(userId, employees);

        result.Should().Be(expected);
    }
}
