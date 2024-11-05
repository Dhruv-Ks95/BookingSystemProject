using BookingSystemProject.Domain.Entities;

namespace BookingSystemProject.Domain.Interfaces;

public interface ISeatRepository
{
    void AddSeat(Seat seat);
    void RemoveSeat(int seatNumber);
    Seat GetSeatById(int seatId);
    List<Seat> GetAllSeats();
    Seat GetSeatByNumber(int seatNumber);
}
