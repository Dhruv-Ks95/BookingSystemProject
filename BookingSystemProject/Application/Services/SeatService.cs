using BookingSystemProject.Application.Interfaces;
using BookingSystemProject.Domain.Entities;
using BookingSystemProject.Domain.Interfaces;

namespace BookingSystemProject.Application.Services;
public class SeatService : ISeatService
{
    private ISeatRepository seatRepository;

    public SeatService(ISeatRepository seatRepository)
    {
        this.seatRepository = seatRepository;
    }

    public Seat CreateASeat(int seatNumber)
    {
        return new Seat(seatNumber);
    }
    public void AddASeat(Seat seat)
    {
        seatRepository.AddSeat(seat);
    }
    public Seat GetSeatBySeatId(int seatId)
    {
        return seatRepository.GetSeatById(seatId);
    }
    public void RemoveASeat(int seatId)
    {
        seatRepository.RemoveSeat(seatId);
    }
    public List<Seat> GetEverySeat()
    {
        return seatRepository.GetAllSeats();
    }
    public bool IsBooked(int seatId)
    {
        Seat st = seatRepository.GetSeatById(seatId);
        return st.IsBooked;
    }
    public bool UpdateSeat(int seatid, int newSeatNumber, bool newSeatStatus)
    {
        Seat st = seatRepository.GetSeatById(seatid);
        if (st != null)
        {
            st.SeatNumber = newSeatNumber;
            st.IsBooked = newSeatStatus;
            return true;
        }
        return false;
    }

}
