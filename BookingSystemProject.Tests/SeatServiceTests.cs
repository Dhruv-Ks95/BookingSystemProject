using Agdata.SeatBookingSystem.Application.Services;
using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Repositories;
using FluentAssertions;

namespace Agdata.SeatBookingSystem.Tests;

public class SeatServiceTests
{
    private SeatService _seatService;
    private SeatRepository _seatRepository;

    public SeatServiceTests()
    {
        _seatRepository = new SeatRepository();
        _seatService = new SeatService(_seatRepository);
    }

    [Fact]
    public void CreateASeat_Should_Return_Seat_With_Correct_Number()
    {
        int seatNumber = 10;

        var result = _seatService.CreateASeat(seatNumber);

        result.SeatNumber.Should().Be(seatNumber);
        result.IsBooked.Should().BeFalse();
    }

    [Fact]
    public void AddASeat_Should_Add_Seat_To_Repository()
    {
        Seat seat = new Seat(5);

        _seatService.AddASeat(seat);

        _seatRepository.GetSeatById(seat.SeatId).Should().Be(seat);
    }

    [Fact]
    public void GetSeatBySeatId_Should_Return_Seat_With_Given_Id()
    {
        Seat seat = new Seat(3);
        _seatRepository.AddSeat(seat);

        var result = _seatService.GetSeatBySeatId(seat.SeatId);

        result.Should().Be(seat);
    }

    [Fact]
    public void RemoveASeat_Should_Remove_Seat_From_Repository()
    {
        Seat seat = new Seat(7);
        _seatRepository.AddSeat(seat);

        _seatService.RemoveASeat(seat.SeatId);

        _seatRepository.GetSeatById(seat.SeatId).Should().BeNull();
    }

    [Fact]
    public void GetEverySeat_Should_Return_All_Seats()
    {
        Seat seat1 = new Seat(1);
        Seat seat2 = new Seat(2);
        _seatRepository.AddSeat(seat1);
        _seatRepository.AddSeat(seat2);

        var result = _seatService.GetEverySeat();

        result.Should().Contain(new List<Seat> { seat1, seat2 });
    }

    [Fact]
    public void IsBooked_Should_Return_True_If_Seat_Is_Booked()
    {
        Seat seat = new Seat(4) { IsBooked = true };
        _seatRepository.AddSeat(seat);

        var result = _seatService.IsBooked(seat.SeatId);

        result.Should().BeTrue();
    }

    [Fact]
    public void UpdateSeat_Should_Update_Seat_Details()
    {
        Seat seat = new Seat(8) { IsBooked = false };
        _seatRepository.AddSeat(seat);

        int newSeatNumber = 88;
        bool newSeatStatus = true;

        var result = _seatService.UpdateSeat(seat.SeatId, newSeatNumber, newSeatStatus);

        result.Should().BeTrue();
        seat.SeatNumber.Should().Be(newSeatNumber);
        seat.IsBooked.Should().Be(newSeatStatus);
    }
}
