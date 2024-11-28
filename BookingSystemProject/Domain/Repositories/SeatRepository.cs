using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Interfaces;
using Agdata.SeatBookingSystem.Infrastructure.Data;
namespace Agdata.SeatBookingSystem.Domain.Repositories;

public class SeatRepository : ISeatRepository
{
    private SeatBookingDbContext context;

    public SeatRepository(SeatBookingDbContext _context)
    {
        context = _context;
    }

    public int AddSeat(Seat seat)
    {
        context.Seats.Add(seat);
        context.SaveChanges();
        return seat.SeatId;
    }

    public Seat GetSeatById(int seatid)
    {
        Seat seat = context.Seats.FirstOrDefault(s => s.SeatId == seatid);
        return seat;
    }

    public bool RemoveSeat(int seatId)
    {
        Seat seatToRemove = GetSeatById(seatId);
        if (seatToRemove != null)
        {
            context.Seats.Remove(seatToRemove);
            var rowsAffected = context.SaveChanges();
            return rowsAffected > 0;
        }   
        return false;
    }

    public IEnumerable<Seat> GetAllSeats()
    {
        return context.Seats.ToList();
    }
}
