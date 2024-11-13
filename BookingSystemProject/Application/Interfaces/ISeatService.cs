using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Application.Interfaces;
public interface ISeatService
{
    Seat CreateASeat(int seatNumber);
    int AddASeat(Seat seat); 
    bool RemoveASeat(int seatId); 
    Seat GetSeatBySeatId(int seatId);
    IEnumerable<Seat> GetEverySeat();    
    bool UpdateSeat(int seatid, int newSeatNumber);

}
