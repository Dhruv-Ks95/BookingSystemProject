using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Domain.Interfaces;

public interface ISeatRepository
{
    int AddSeat(Seat seat); 
    bool RemoveSeat(int seatNumber); 
    Seat GetSeatById(int seatId);
    IEnumerable<Seat> GetAllSeats();
}
