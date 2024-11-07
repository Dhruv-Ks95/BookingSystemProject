using BookingSystemProject.Application.Interfaces;
using BookingSystemProject.Application.Services;
using BookingSystemProject.Domain.Entities;
using BookingSystemProject.Domain.Interfaces;
using BookingSystemProject.Domain.Repositories;

namespace BookingSystemProject.Presentation.ConsoleApp;

internal class Program
{

    public static BookingService bookingService;
    private static SeatService seatService;
    private static EmployeeService employeeService;

    static void Main(string[] args)
    {
        // -----------------INITIALIZING------------------------START

        IEmployeeRepository employeeRepository = new EmployeeRepository();
        ISeatRepository seatRepository = new SeatRepository();
        IBookingRepository bookingRepository = new BookingRepository();
        IValidationService validationService = new ValidationService();

        bookingService = new BookingService(bookingRepository, seatRepository);
        employeeService = new EmployeeService(employeeRepository);
        seatService = new SeatService(seatRepository);


        for (int i = 1; i < 11; i++) // Add seats to the repository using the service 
        {
            seatService.AddASeat(seatService.CreateASeat(i));
        }


        // Create and Add employees to employee repository using employeeService 
        Employee user = employeeService.CreateAnEmployee("Dhruv", "dhruvk@company.com", RoleType.User);
        Employee admin = employeeService.CreateAnEmployee("Admin", "admin@company.com", RoleType.Admin);

        // List of employees in the company
        employeeService.AddAnEmployee(user);
        employeeService.AddAnEmployee(admin);
        employeeService.AddAnEmployee(employeeService.CreateAnEmployee("Mr.Shukla", "shukla@company.com", RoleType.User));
        employeeService.AddAnEmployee(employeeService.CreateAnEmployee("Mr.Das", "das@company.com", RoleType.User));
        employeeService.AddAnEmployee(employeeService.CreateAnEmployee("Mr.Reddy", "reddy@company.com", RoleType.User));

        MenuMethods menuMethods = new MenuMethods(bookingService, validationService, employeeService);

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
                    menuMethods.UserMenu(user);
                    break;
                case "2":
                    menuMethods.AdminMenu(admin);
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
