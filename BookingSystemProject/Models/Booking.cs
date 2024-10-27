using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemProject.Models
{
    public class Booking
    {
        private static int nextBookingId = 1;

        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int SeatNumber { get; set; }
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
