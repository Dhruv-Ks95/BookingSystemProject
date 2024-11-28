using Agdata.SeatBookingSystem.Application.Interfaces;
using Agdata.SeatBookingSystem.Application.Services;
using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Interfaces;
using Agdata.SeatBookingSystem.Domain.Repositories;
using Agdata.SeatBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agdata.SeatBookingSystem.Presentation.ConsoleApp;

internal class Program
{

    public static BookingService bookingService;
    private static SeatService seatService;
    private static EmployeeService employeeService;

    static void Main(string[] args)
    {
        // -----------------INITIALIZING------------------------START
        
        using SeatBookingDbContext context = new SeatBookingDbContext();

        IEmployeeRepository employeeRepository = new EmployeeRepository(context);
        ISeatRepository seatRepository = new SeatRepository(context);
        IBookingRepository bookingRepository = new BookingRepository(context);

        IValidationService validationService = new ValidationService();

        employeeService = new EmployeeService(employeeRepository);
        seatService = new SeatService(seatRepository);
        bookingService = new BookingService(bookingRepository, seatRepository, employeeService);

        Employee admin = employeeService.GetEmployeeByEmployeeId(2);
        Employee user = employeeService.GetEmployeeByEmployeeId(6);
     
        MenuMethods menuMethods = new MenuMethods(bookingService, validationService, employeeService, seatService);

        // -----------------INITIALIZING------------------------END


        // -----------------MAIN PROGRAM------------------------START
        bool exitProgram = false;
        while (!exitProgram)
        {
            Console.WriteLine("\n Welcome to the Booking System");
            Console.WriteLine("1. User Login");
            Console.WriteLine("2. Admin Login");
            Console.WriteLine("3. Exit");
            Console.Write("Please select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    menuMethods.UserMenu(user.EmployeeId);
                    break;
                case "2":
                    menuMethods.AdminMenu(admin.EmployeeId);
                    break;
                case "3":
                    exitProgram = true;
                    Console.WriteLine("Thank you for using the Booking System!");
                    break;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    break;
            }
        }

        // -----------------MAIN PROGRAM------------------------END
    }


}
