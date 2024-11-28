using Agdata.SeatBookingSystem.Application.Services;
using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Repositories;
using Agdata.SeatBookingSystem.Infrastructure.Data;
using Agdata.SeatBookingSystem.Tests.Mocks;
using FluentAssertions;

namespace Agdata.SeatBookingSystem.Tests;

public class SeatServiceTests
{
    SeatBookingDbContext dbContext = MockDbContext.GetDbContextWithTestData();
    private SeatService _seatService;
    private SeatRepository _seatRepository;
    public SeatServiceTests()
    {
        _seatRepository = new SeatRepository(dbContext);
        _seatService = new SeatService(_seatRepository);
    }    

    [Fact]
    public void AddASeat_Should_Add_Seat_To_Repository()
    {
        Seat seat = new Seat(11);

        _seatService.AddASeat(seat);

        _seatRepository.GetSeatById(seat.SeatId).Should().Be(seat);
    }

    [Fact]
    public void GetSeatBySeatId_Should_Return_Seat_With_Given_Id()
    {
        Seat seat = new Seat(12);
        _seatRepository.AddSeat(seat);

        var result = _seatService.GetSeatBySeatId(seat.SeatId);

        result.Should().Be(seat);
    }

    [Fact]
    public void RemoveASeat_Should_Remove_Seat_From_Repository()
    {
        Seat seat = new Seat(13);
        _seatRepository.AddSeat(seat);

        _seatService.RemoveASeat(seat.SeatId);

        _seatRepository.GetSeatById(seat.SeatId).Should().BeNull();
    }

    [Fact]
    public void GetEverySeat_Should_Return_All_Seats()
    {

        var listOfSeats = _seatService.GetEverySeat();
        int result = listOfSeats.Count();

        result.Should().BeGreaterThan(0);
    }
}
