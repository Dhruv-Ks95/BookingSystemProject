using Agdata.SeatBookingSystem.Application.Interfaces;
using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Presentation.ConsoleApp;
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
    public void UserMenu(int empId)
    {
        bool hasLoggedOut = false;
        Employee employee = employeeService.GetEmployeeByEmployeeId(empId);
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
                    BookSeat(empId);
                    break;
                case "2":
                    ViewUserBookings(empId);
                    break;
                case "3":
                    CancelUserBooking(empId);
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

    public void AdminMenu(int adminId)
    {
        bool hasLoggedOut = false;
        Employee admin = employeeService.GetEmployeeByEmployeeId(adminId);
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
                    BookSeatForEmployee(adminId); // Now uses Employee
                    break;
                case "2":
                    ViewAllBookings(adminId);
                    break;
                case "3":
                    ModifyBooking(adminId);
                    break;
                case "4":
                    DeleteBooking(adminId);
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
    private void BookSeat(int empId)
    {
        Console.Write("Enter the date for booking (yyyy-MM-dd): ");
        string bookingDate = Console.ReadLine();
        if (!validationService.IsValidDate(bookingDate))
        {
            consoleOutputService.PrintError("Invalid Booking Date");
            return;
        }
        DateTime.TryParse(bookingDate, out DateTime bookingDay);
        IEnumerable<Seat> availableSeats = bookingService.GetSeatsOnGivenDay(bookingDay);
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
        int bknid = bookingService.BookSeat(empId, bookingDay, seatNumber);
        consoleOutputService.PrintSuccess("Booking successful with ID : " + bknid);
        return;
    }

    public void ViewUserBookings(int empId)
    {
        IEnumerable<Booking> userBookings = bookingService.GetUserBookings(empId);

        if (validationService.IsEmptyList(userBookings))
        {
            consoleOutputService.PrintWarning("You have no bookings in the next 30 days.");
            return;
        }
        consoleOutputService.PrintBookings(userBookings);
    }

    public void CancelUserBooking(int empId)
    {
        IEnumerable<Booking> userBookings = bookingService.GetUserBookings(empId);

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

        if (bookingService.CancelUserBookings(empId, bookingId))
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
    public void BookSeatForEmployee(int adminId)
    {
        Console.Write("\nEnter the date for booking (yyyy-MM-dd): ");
        string dateToBookOn = Console.ReadLine();
        if (!validationService.IsValidDate(dateToBookOn))
        {
            consoleOutputService.PrintError("Invalid Date!");
            return;
        }
        DateTime.TryParse(dateToBookOn, out DateTime bookingDate);
        IEnumerable<Seat> availableSeats = bookingService.GetSeatsOnGivenDay(bookingDate);
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
        IEnumerable<Employee> employees = employeeService.GetAllEmployees();
        consoleOutputService.PrintEmployees(employees);
        Console.WriteLine("Select User by thier Employee ID: ");
        string userToBeSelected = Console.ReadLine();
        if (!validationService.IsValidUser(userToBeSelected, employees))
        {
            consoleOutputService.PrintError("Invalid Employee Id provided! Try again");
            return;
        }
        int.TryParse(userToBeSelected, out int userId);
        int bknid = bookingService.BookSeatForEmployee(adminId, userId, bookingDate, seatNumber);        
        consoleOutputService.PrintSuccess("Booking successful with ID : " + bknid);
        return;

    }

    public void ModifyBooking(int adminId)
    {
        Console.Write("Enter the date of the booking to modify (yyyy-MM-dd): ");
        string dateToSearch = Console.ReadLine();
        if (!validationService.IsValidDate(dateToSearch))
        {
            consoleOutputService.PrintError("Invalid Date Entered!");
            return;
        }
        DateTime.TryParse(dateToSearch, out DateTime bookingDate);
        IEnumerable<Booking> bookingsOnSelectedDate = bookingService.GetAllBookingsOnDate(adminId, bookingDate);
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
        IEnumerable<Seat> availableSeats = bookingService.GetSeatsOnGivenDay(bookingDate);
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
        bookingService.ModifyAnyBooking(adminId, bookingDate, bookingId, seatNumber);
        consoleOutputService.PrintSuccess("Booking modified Successfully!");
    }

    public void ViewAllBookings(int adminId)
    {
        Console.Write("Enter the date to view bookings (yyyy-MM-dd): ");
        string bookingDateToSearch = Console.ReadLine();
        if (!validationService.IsValidDate(bookingDateToSearch))
        {
            consoleOutputService.PrintError("Invalid Date!");
            return;
        }
        DateTime bookingDate = DateTime.Parse(bookingDateToSearch);
        IEnumerable<Booking> bookingsOnDate = bookingService.GetAllBookingsOnDate(adminId, bookingDate);
        if (validationService.IsEmptyList(bookingsOnDate))
        {
            consoleOutputService.PrintWarning("No Bookings on Selected Date!");
            return;
        }
        consoleOutputService.PrintBookings(bookingsOnDate);

    }

    public void DeleteBooking(int adminId)
    {
        Console.Write("Enter the date of the booking to delete (yyyy-MM-dd): ");
        string dateToSearch = Console.ReadLine();
        if (!validationService.IsValidDate(dateToSearch))
        {
            consoleOutputService.PrintError("Invalid Date!");
            return;
        }
        DateTime.TryParse(dateToSearch, out DateTime bookingDate);
        IEnumerable<Booking> bookingsOnDate = bookingService.GetAllBookingsOnDate(adminId, bookingDate);
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
            if (bookingService.CancelAnyBooking(adminId, bookingId))
            {
                consoleOutputService.PrintSuccess("Booking Cancelled Successfully!");
            }
            else
            {
                consoleOutputService.PrintError("Booking cancellation Unsuccessful!");
            }
        }
        catch (Exception ex)
        {
            consoleOutputService.PrintError("Booking couldn't be cancelled: ");
            consoleOutputService.PrintError($"{ex.Message}");
        }

    }
}
