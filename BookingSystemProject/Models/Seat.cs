using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemProject.Models
{
    public class Seat
    {
        public int SeatNumber { get; set; }
        public bool IsBooked { get; set; }
        public Seat(int seatNumber)
        {
            SeatNumber = seatNumber;
            IsBooked = false;
        }
    }
}
