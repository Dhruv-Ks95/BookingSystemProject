using BookingSystemProject.Models;

namespace BookingSystemProject.Utilities;

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

    public void PrintSeats(List<Seat> seats)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Available seats:");
        foreach (var seat in seats)
        {
            Console.WriteLine($"Seat Number: {seat.SeatNumber}");
        }
        Console.ResetColor();
    }

    public void PrintBookings(List<Booking> bookings)
    {
        Console.ForegroundColor= ConsoleColor.Yellow;
        Console.WriteLine("Bookings: ");
        foreach (var booking in bookings)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Booking ID: {booking.BookingId}, Seat Number: {booking.SeatNumber}, Date: {booking.BookingDate.ToShortDateString()}, UserId: {booking.UserId}");
            Console.ResetColor();
        }
    }

    public void PrintEmployees(List<Employee> employees)
    {
        Console.WriteLine("List of employees :");
        foreach(var employee in employees)
        {
            Console.WriteLine($"Employee ID: {employee.EmployeeId}, Employee Name : {employee.Name}, Employee Email: {employee.Email} ");
        }
    }



}
