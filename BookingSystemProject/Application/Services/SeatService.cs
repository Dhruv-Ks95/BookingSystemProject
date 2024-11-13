using Agdata.SeatBookingSystem.Application.Interfaces;
using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Interfaces;

namespace Agdata.SeatBookingSystem.Application.Services;
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
    public int AddASeat(Seat seat)
    {
        seatRepository.AddSeat(seat);
        return seat.SeatId;
    }
    public Seat GetSeatBySeatId(int seatId)
    {
        return seatRepository.GetSeatById(seatId);
    }
    public bool RemoveASeat(int seatId)
    {
        seatRepository.RemoveSeat(seatId);
        return true;
    }
    public IEnumerable<Seat> GetEverySeat()
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
