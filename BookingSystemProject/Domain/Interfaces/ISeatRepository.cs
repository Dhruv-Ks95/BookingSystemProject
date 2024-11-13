using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Domain.Interfaces;

public interface ISeatRepository
{
    void AddSeat(Seat seat); // return seat id
    void RemoveSeat(int seatNumber); // return boolean
    Seat GetSeatById(int seatId);
    IEnumerable<Seat> GetAllSeats();
    Seat GetSeatByNumber(int seatNumber); // no need
}
