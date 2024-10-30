using BookingSystemProject.Models;
namespace BookingSystemProject.Services;
public interface ISeatService
{
    Seat CreateASeat(int seatNumber);
    void AddASeat(Seat seat);
    void RemoveASeat(int seatId);
    Seat GetSeatBySeatId(int seatId);
    List<Seat> GetEverySeat();
    bool IsBooked(int seatId);
    bool UpdateSeat(int seatid, int newSeatNumber, bool newSeatStatus);

}
