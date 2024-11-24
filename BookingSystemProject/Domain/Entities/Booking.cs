namespace Agdata.SeatBookingSystem.Domain.Entities
{
    public class Booking
    {
        private static int nextBookingId = 1;

        public int BookingId { get; set; }
        public int EmployeeId { get; set; }
        public int SeatId { get; set; }
        //public int SeatNumber { get; set; }
        public DateTime BookingDate { get; set; }

        public Booking(int userId, DateTime bookingDate, int seatId)
        {
            BookingId = nextBookingId++;
            EmployeeId = userId;
            //SeatNumber = seatNumber;
            SeatId = seatId;
            BookingDate = bookingDate;
        }
    }
}
