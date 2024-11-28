using Agdata.SeatBookingSystem.Application.Interfaces;
using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Presentation.ConsoleApp;

public class ConsoleOutputService
{

    public void PrintSuccess(string str)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(str);
        Console.ResetColor();
    }

    public void PrintError(string str)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(str);
        Console.ResetColor();
    }

    public void PrintWarning(string str)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(str);
        Console.ResetColor();
    }

    public void PrintDanger(string str)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(str);
        Console.ResetColor();
    }

    public void PrintSeats(IEnumerable<Seat> seats)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Available seats:");
        foreach (var seat in seats)
        {
            Console.WriteLine($"Seat Number: {seat.SeatNumber}");
        }
        Console.ResetColor();
    }

    public void PrintBookings(IEnumerable<Booking> bookings, IEnumerable<Employee> users, IEnumerable<Seat> seats)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Bookings: ");
        foreach (var booking in bookings)
        {
            Employee emp = users.FirstOrDefault(e => e.EmployeeId == booking.EmployeeId);
            Seat st = seats.FirstOrDefault(s => s.SeatId == booking.SeatId);
            string empName = emp.Name;
            int stNum = st.SeatNumber;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Booking ID: {booking.BookingId}, Date: {booking.BookingDate.ToShortDateString()}, User Name: {empName}, Seat Number : {stNum}");
            Console.ResetColor();
        }
    }

    public void PrintEmployees(IEnumerable<Employee> employees)
    {

        Console.WriteLine("List of employees :");
        foreach (var employee in employees)
        {
            Console.WriteLine($"Employee ID: {employee.EmployeeId}, Employee Name : {employee.Name}, Employee Email: {employee.Email} ");
        }
    }



}
