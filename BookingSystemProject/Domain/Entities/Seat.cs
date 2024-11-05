using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemProject.Domain.Entities
{
    public class Seat
    {
        private static int nextSeatId = 1;
        public int SeatId { get; private set; }
        public int SeatNumber { get; set; }
        public bool IsBooked { get; set; }
        public Seat(int seatNumber)
        {
            SeatId = nextSeatId++;
            SeatNumber = seatNumber;
            IsBooked = false;
        }
    }
}
