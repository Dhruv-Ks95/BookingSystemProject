using BookingSystemProject.Domain.Entities;
using BookingSystemProject.Domain.Interfaces;
using System.Runtime.Serialization;
namespace BookingSystemProject.Domain.Repositories;

public class SeatRepository : ISeatRepository
{
    private List<Seat> _seatList;

    public SeatRepository()
    {
        _seatList = new List<Seat>();
    }

    public void AddSeat(Seat seat)
    {
        _seatList.Add(seat);
    }

    public void RemoveSeat(int seatId)
    {
        _seatList.RemoveAll(s => s.SeatId == seatId);
    }

    public Seat GetSeatByNumber(int seatNumber)
    {
        return _seatList.Find(s => s.SeatNumber == seatNumber);
    }

    public Seat GetSeatById(int seatid)
    {
        return _seatList.Find(s => s.SeatId == seatid);
    }

    public List<Seat> GetAllSeats()
    {
        return _seatList;
    }




}
