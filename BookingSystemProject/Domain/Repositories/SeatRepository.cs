using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Interfaces;
namespace Agdata.SeatBookingSystem.Domain.Repositories;

public class SeatRepository : ISeatRepository
{
    private List<Seat> _seatList;

    public SeatRepository()
    {
        _seatList = new List<Seat>();
    }

    public int AddSeat(Seat seat)
    {
        _seatList.Add(seat);
        return seat.SeatId;
    }

    public bool RemoveSeat(int seatId)
    {
        _seatList.RemoveAll(s => s.SeatId == seatId);
        return true;
    }

    public Seat GetSeatById(int seatid)
    {
        return _seatList.Find(s => s.SeatId == seatid);
    }

    public IEnumerable<Seat> GetAllSeats()
    {
        return _seatList;
    }
}
