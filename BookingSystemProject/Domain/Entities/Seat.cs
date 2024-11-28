namespace Agdata.SeatBookingSystem.Domain.Entities
{
    public class Seat
    {
        //private static int nextSeatId = 1;
        public int SeatId { get; set; }
        public int SeatNumber { get; set; }
        
        public Seat(int seatNumber)
        {
            //SeatId = nextSeatId++;
            SeatNumber = seatNumber;            
        }

        public Seat() { }
    }
}
