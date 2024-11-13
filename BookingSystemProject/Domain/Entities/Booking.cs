namespace Agdata.SeatBookingSystem.Domain.Entities
{
    public class Booking
    {
        private static int nextBookingId = 1;

        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int SeatNumber { get; set; } // seat id too maybe
        public DateTime BookingDate { get; set; }

        public Booking(int userId, int seatNumber, DateTime bookingDate)
        {
            BookingId = nextBookingId++;
            UserId = userId;
            SeatNumber = seatNumber;
            BookingDate = bookingDate;
        }
    }
}
