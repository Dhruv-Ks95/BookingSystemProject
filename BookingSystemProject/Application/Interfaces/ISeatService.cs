using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Application.Interfaces;
public interface ISeatService
{
    int AddASeat(Seat seat); 
    bool RemoveASeat(int seatId); 
    Seat GetSeatBySeatId(int seatId);
    IEnumerable<Seat> GetEverySeat();    
    bool UpdateSeat(int seatid, int newSeatNumber);

}
