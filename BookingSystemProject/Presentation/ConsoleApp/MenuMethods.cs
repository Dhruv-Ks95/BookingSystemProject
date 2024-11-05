using BookingSystemProject.Application.Interfaces;
using BookingSystemProject.Domain.Entities;
using System.Reflection.Metadata;

namespace BookingSystemProject.Presentation.ConsoleApp;
public class MenuMethods
{
    private IBookingService bookingService;
    private IValidationService validationService;
    private IEmployeeService employeeService;
    private ConsoleOutputService consoleOutputService = new ConsoleOutputService();
    public MenuMethods(IBookingService bookingService, IValidationService validationService, IEmployeeService employeeService)
    {
        this.bookingService = bookingService;
        this.validationService = validationService;
        this.employeeService = employeeService;
    }

    // DASHBOARDS
    public void UserMenu(Employee employee)
    {
        bool hasLoggedOut = false;
        while (!hasLoggedOut)
        {
            Console.WriteLine($"\n Hello, {employee.Name}! What would you like to do?");
            Console.WriteLine("1. Book a Seat");
            Console.WriteLine("2. View My Bookings");
            Console.WriteLine("3. Cancel a Booking");
            Console.WriteLine("4. Log Out");
            Console.Write("Please select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    BookSeat(employee);
                    break;
                case "2":
                    ViewUserBookings(employee);
                    break;
                case "3":
                    CancelUserBooking(employee);
                    break;
                case "4":
                    Console.WriteLine("Logging out.");
                    hasLoggedOut = true;
                    break;
                default:
                    consoleOutputService.PrintError("\n Invalid input. Please try again.\n");
                    break;
            }
        }
        return;
    }

    public void AdminMenu(Employee admin, List<Employee> employees)
    {
        bool hasLoggedOut = false;
        while (!hasLoggedOut)
        {
            Console.WriteLine($"\nHello, {admin.Name}! What would you like to do?");
            Console.WriteLine("1. Book a Seat for a User");
            Console.WriteLine("2. View All Bookings");
            Console.WriteLine("3. Modify a Booking");
            Console.WriteLine("4. Cancel a Booking");
            Console.WriteLine("5. Log Out");
            Console.Write("Please select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    BookSeatForEmployee(admin); // Now uses Employee
                    break;
                case "2":
                    ViewAllBookings(admin);
                    break;
                case "3":
                    ModifyBooking(admin);
                    break;
                case "4":
                    DeleteBooking(admin);
                    break;
                case "5":
                    Console.WriteLine("Logging out.");
                    hasLoggedOut = true;
                    break;

                default:
                    consoleOutputService.PrintError("Invalid input. Please try again.");
                    break;
            }
        }
    }
    // USER FUNCTIONS -> add colors in bookseat
    private void BookSeat(Employee employee)
    {
        Console.Write("Enter the date for booking (yyyy-MM-dd): ");
        string bookingDate = Console.ReadLine();
        if (!validationService.IsValidDate(bookingDate))
        {
            consoleOutputService.PrintError("Invalid Booking Date");
            return;
        }
        DateTime.TryParse(bookingDate, out DateTime bookingDay);
        List<Seat> availableSeats = bookingService.GetSeatsOnGivenDay(bookingDay);
        if (validationService.IsEmptyList(availableSeats))
        {
            consoleOutputService.PrintError("No Seats available on Choosen Day");
            return;
        }
        consoleOutputService.PrintSeats(availableSeats);
        Console.WriteLine("Please select a seat Number from the list :");
        string selectedSeatNumber = Console.ReadLine();
        if (!validationService.IsValidSeatNumber(selectedSeatNumber, availableSeats))
        {
            consoleOutputService.PrintError("Invalid seat number");
            return;
        }
        int.TryParse(selectedSeatNumber, out int seatNumber);
        if (bookingService.BookSeat(employee, bookingDay, seatNumber))
        {
            consoleOutputService.PrintSuccess("Seat booked Successfully!");
            return;
        }
        else
        {
            consoleOutputService.PrintError("Seat could not be booked!");
        }
        return;
    }

    public void ViewUserBookings(Employee employee)
    {
        List<Booking> userBookings = bookingService.GetUserBookings(employee);

        if (validationService.IsEmptyList(userBookings))
        {
            consoleOutputService.PrintWarning("You have no bookings in the next 30 days.");
            return;
        }
        consoleOutputService.PrintBookings(userBookings);
    }

    public void CancelUserBooking(Employee employee)
    {
        List<Booking> userBookings = bookingService.GetUserBookings(employee);

        if (validationService.IsEmptyList(userBookings))
        {
            consoleOutputService.PrintWarning("You have no bookings to cancel in the next 30 days.");
            return;
        }
        consoleOutputService.PrintBookings(userBookings);

        consoleOutputService.PrintDanger("Enter the Booking ID to cancel: ");

        string bookinId = Console.ReadLine();
        if (!validationService.IsValidBookingId(bookinId, userBookings))
        {
            consoleOutputService.PrintError("Invalid Booking ID");
            return;
        }
        int.TryParse(bookinId, out int bookingId);

        if (bookingService.CancelUserBookings(employee, bookingId))
        {
            consoleOutputService.PrintSuccess("Booking cancelled successfully!");
        }
        else
        {
            consoleOutputService.PrintError("Cancellation unsuccessful!");
        }
        return;

    }

    // ADMIN FUNCTIONS
    public void BookSeatForEmployee(Employee admin)
    {
        Console.Write("\nEnter the date for booking (yyyy-MM-dd): ");
        string dateToBookOn = Console.ReadLine();
        if (!validationService.IsValidDate(dateToBookOn))
        {
            consoleOutputService.PrintError("Invalid Date!");
            return;
        }
        DateTime.TryParse(dateToBookOn, out DateTime bookingDate);
        List<Seat> availableSeats = bookingService.GetSeatsOnGivenDay(bookingDate);
        if (validationService.IsEmptyList(availableSeats))
        {
            consoleOutputService.PrintWarning("All seats booked on selected Date!");
            return;
        }
        consoleOutputService.PrintSeats(availableSeats);
        Console.WriteLine("Please select a Seat Number from the list to Book ");
        string selectedSeatNumber = Console.ReadLine();
        if (!validationService.IsValidSeatNumber(selectedSeatNumber, availableSeats))
        {
            consoleOutputService.PrintError("Invalid Seat number selected!");
            return;
        }
        int.TryParse(selectedSeatNumber, out int seatNumber);
        List<Employee> employees = employeeService.GetAllEmployees();
        consoleOutputService.PrintEmployees(employees);
        Console.WriteLine("Select User by thier Employee ID: ");
        string userToBeSelected = Console.ReadLine();
        if (!validationService.IsValidUser(userToBeSelected, employees))
        {
            consoleOutputService.PrintError("Invalid Employee Id provided! Try again");
            return;
        }
        int.TryParse(userToBeSelected, out int userId);
        Employee userToBookFor = employeeService.GetEmployeeByEmployeeId(userId);
        if (bookingService.BookSeatForEmployee(admin, userToBookFor, bookingDate, seatNumber))
        {
            consoleOutputService.PrintSuccess("Booking Successful!");
        }
        else
        {
            consoleOutputService.PrintError("Unable To book!");
        }

    }

    public void ModifyBooking(Employee admin)
    {
        Console.Write("Enter the date of the booking to modify (yyyy-MM-dd): ");
        string dateToSearch = Console.ReadLine();
        if (!validationService.IsValidDate(dateToSearch))
        {
            consoleOutputService.PrintError("Invalid Date Entered!");
            return;
        }
        DateTime.TryParse(dateToSearch, out DateTime bookingDate);
        List<Booking> bookingsOnSelectedDate = bookingService.GetAllBookingsOnDate(admin, bookingDate);
        if (validationService.IsEmptyList(bookingsOnSelectedDate))
        {
            consoleOutputService.PrintError("No Bookings on selected Date!");
            return;
        }
        consoleOutputService.PrintBookings(bookingsOnSelectedDate);
        Console.WriteLine("Enter a Booking ID to modify from the above list: ");
        string bookingIdSelected = Console.ReadLine();
        if (!validationService.IsValidBookingId(bookingIdSelected, bookingsOnSelectedDate))
        {
            consoleOutputService.PrintError("Invalid Booking ID!");
            return;
        }
        int.TryParse(bookingIdSelected, out int bookingId);
        List<Seat> availableSeats = bookingService.GetSeatsOnGivenDay(bookingDate);
        if (validationService.IsEmptyList(availableSeats))
        {
            consoleOutputService.PrintError("No seats available to choose!");
            return;
        }
        consoleOutputService.PrintSeats(availableSeats);
        Console.WriteLine("Select a enw seat from the List: ");
        string selectedSeatNumber = Console.ReadLine();
        if (!validationService.IsValidSeatNumber(selectedSeatNumber, availableSeats))
        {
            consoleOutputService.PrintError("Invalid Seat Number provided!");
            return;
        }
        int.TryParse(selectedSeatNumber, out int seatNumber);
        bookingService.ModifyAnyBooking(admin, bookingDate, bookingId, seatNumber);
        consoleOutputService.PrintSuccess("Booking modified Successfully!");
    }

    public void ViewAllBookings(Employee admin)
    {
        Console.Write("Enter the date to view bookings (yyyy-MM-dd): ");
        string bookingDateToSearch = Console.ReadLine();
        if (!validationService.IsValidDate(bookingDateToSearch))
        {
            consoleOutputService.PrintError("Invalid Date!");
            return;
        }
        DateTime.TryParse(bookingDateToSearch, out DateTime bookingDate);
        List<Booking> bookingsOnDate = bookingService.GetAllBookingsOnDate(admin, bookingDate);
        if (validationService.IsEmptyList(bookingsOnDate))
        {
            consoleOutputService.PrintWarning("No Bookings on Selected Date!");
            return;
        }
        consoleOutputService.PrintBookings(bookingsOnDate);

    }

    public void DeleteBooking(Employee admin)
    {
        Console.Write("Enter the date of the booking to delete (yyyy-MM-dd): ");
        string dateToSearch = Console.ReadLine();
        if (!validationService.IsValidDate(dateToSearch))
        {
            consoleOutputService.PrintError("Invalid Date!");
            return;
        }
        DateTime.TryParse(dateToSearch, out DateTime bookingDate);
        List<Booking> bookingsOnDate = bookingService.GetAllBookingsOnDate(admin, bookingDate);
        if (validationService.IsEmptyList(bookingsOnDate))
        {
            consoleOutputService.PrintError("No Bookings on selected Date!");
            return;
        }
        consoleOutputService.PrintBookings(bookingsOnDate);
        consoleOutputService.PrintDanger("Select Booking Id to delete from the list :");
        string bookingIdToSelect = Console.ReadLine();
        if (!validationService.IsValidBookingId(bookingIdToSelect, bookingsOnDate))
        {
            consoleOutputService.PrintError("Inavlid Booking ID!");
            return;
        }
        int.TryParse(bookingIdToSelect, out int bookingId);
        try
        {
            bookingService.CancelAnyBooking(admin, bookingId);
            consoleOutputService.PrintSuccess("Booking Cancelled Successfully!");
        }
        catch (Exception ex)
        {
            consoleOutputService.PrintError("Booking couldn't be cancelled: ");
            consoleOutputService.PrintError($"{ex.Message}");
        }

    }
}
