namespace Agdata.SeatBookingSystem.Domain.Entities
{
    public class Seat
    {
        private static int nextSeatId = 1;
        public int SeatId { get; private set; }
        public int SeatNumber { get; set; }
        public bool IsBooked { get; set; } // is wrong
        public Seat(int seatNumber)
        {
            SeatId = nextSeatId++;
            SeatNumber = seatNumber;
            IsBooked = false;
        }
    }
}
