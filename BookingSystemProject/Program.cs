using BookingSystemProject.Models;
using BookingSystemProject.Services;

namespace BookingSystemProject;

internal class Program
{

    private static BookingService bookingService;

    static void Main(string[] args)
    {
        // -----------------INITIALIZING------------------------START

        List<Seat> seats = new List<Seat>();  // Initialize some seats for office
        for (int i = 1; i <= 10; i++)
        {
            seats.Add(new Seat(i));
        }

        List<Booking> bookings = new List<Booking>(); // Initializing Booking storage

        bookingService = new BookingService(bookings, seats);

        // Creating sample employees (using Employee class)
        Employee user = new Employee("Dhruv", "dhruvk@company.com", RoleType.User);
        Employee admin = new Employee("Admin", "admin@company.com", RoleType.Admin);

        // Creating a list of employees in the company
        List<Employee> employees = new List<Employee>();
        employees.Add(user);
        employees.Add(new Employee("Mr.Shukla", "shukla@company.com", RoleType.User));
        employees.Add(new Employee("Mr.Das", "das@company.com", RoleType.User));
        employees.Add(new Employee("Mr.Reddy", "reddy@company.com", RoleType.User));

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
                    UserMenu(user);
                    break;
                case "2":                    
                    AdminMenu(admin, employees);
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

    // DASHBOARDS
    static void UserMenu(Employee employee)
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Invalid input. Please try again.\n");
                    Console.ResetColor();
                    break;
            }
        }
        return;
    }

    static void AdminMenu(Employee admin, List<Employee> employees)
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
                    BookSeatForEmployee(admin, employees); // Now uses Employee
                    break;
                case "2":
                    ViewAllBookings(admin, employees);
                    break;
                case "3":
                    ModifyBooking(admin, employees);
                    break;
                case "4":
                    DeleteBooking(admin, employees);
                    break;
                case "5":
                    Console.WriteLine("Logging out.");
                    hasLoggedOut = true;
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ResetColor();
                    break;
            }
        }
    }
    // USER FUNCTIONS
    static void BookSeat(Employee employee)
    {
        Console.Write("Enter the date for booking (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime bookingDate))
        {
            // Validating Date
            if (bookingDate < DateTime.Today || bookingDate > DateTime.Today.AddDays(30))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n You can only book a seat within the next 30 days. \n");
                Console.ResetColor();
                return;
            }

            // Show available seats on the selected date
            var availableSeats = bookingService.GetSeatsOnGivenDay(bookingDate);

            if (availableSeats.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo seats are available on the selected date.\n ");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Available seats:");
            foreach (var seat in availableSeats)
            {
                Console.WriteLine($"Seat Number: {seat.SeatNumber}");
            }
            Console.ResetColor();

            bool isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("Please enter a seat number:");
                if (int.TryParse(Console.ReadLine(), out int seatNumber))
                {
                    Seat selectedSeat = null;
                    foreach (var seat in availableSeats)
                    {
                        if (seat.SeatNumber == seatNumber)
                        {
                            selectedSeat = seat;
                            break;
                        }
                    }
                    if (selectedSeat != null)
                    {
                        bool result = bookingService.BookSeat(employee, bookingDate, seatNumber);
                        Console.ForegroundColor = ConsoleColor.Green;
                        if(result)
                        {
                            Console.WriteLine($"Booking successful!");
                        }
                        Console.ResetColor();
                        isValidInput = true;
                    }
                    else
                    {
                        Console.WriteLine("\n Invalid seat number. Please try again. \n ");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid seat number.");
                }
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid date format. Please enter a date in yyyy-MM-dd format.");
            Console.ResetColor();
        }
    }

    static void ViewUserBookings(Employee employee)
    {
        List<Booking> userBookings = bookingService.GetUserBookings(employee);

        if (userBookings.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You have no bookings in the next 30 days.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine("Your bookings:");
        foreach (var booking in userBookings)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Booking ID: {booking.BookingId}, Seat Number: {booking.SeatNumber}, Date: {booking.BookingDate.ToShortDateString()}");
            Console.ResetColor();
        }
    }

    static void CancelUserBooking(Employee employee)
    {
        List<Booking> userBookings = bookingService.GetUserBookings(employee);

        if (userBookings.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You have no bookings to cancel in the next 30 days.");
            Console.ResetColor();
            return;
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Your bookings:");
        foreach (var booking in userBookings)
        {
            Console.WriteLine($"Booking ID: {booking.BookingId}, Seat Number: {booking.SeatNumber}, Date: {booking.BookingDate.ToShortDateString()}");
        }
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("Enter the Booking ID to cancel: ");
        Console.ResetColor();

        if (int.TryParse(Console.ReadLine(), out int bookingId))
        {
            if (bookingService.CancelUserBookings(employee, bookingId))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Booking cancelled successfully");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("Provided Booking ID does not exists");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please enter a valid Booking ID in correct format.");
            Console.ResetColor();
        }
    }

    // ADMIN FUNCTIONS
    static void BookSeatForEmployee(Employee admin,  List<Employee> users)
    {
        Console.Write("\nEnter the date for booking (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime bookingDate))
        {
            // Check if the booking date is within the next 30 days
            if (bookingDate < DateTime.Today || bookingDate > DateTime.Today.AddDays(30))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You can only book a seat within the next 30 days.");
                Console.ResetColor();
                return;
            }

            // Show available seats on the selected date -> getSeatsOnSpecifiedDate?
            var availableSeats = bookingService.GetSeatsOnGivenDay(bookingDate);

            if (availableSeats.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo seats are available on the selected date.\n ");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Available seats:");
            foreach (var seat in availableSeats)
            {
                Console.WriteLine($"Seat Number: {seat.SeatNumber}");
            }
            Console.ResetColor();

            // Choose a seat
            bool isValidSeat = false;
            Seat selectedSeat = null;

            while (!isValidSeat)
            {
                Console.Write("Enter the seat number you wish to book: ");
                if (int.TryParse(Console.ReadLine(), out int seatNumber))
                {
                    foreach (var seat in availableSeats)
                    {
                        if (seat.SeatNumber == seatNumber)
                        {
                            selectedSeat = seat;
                            isValidSeat = true;
                            break;
                        }
                    }

                    if (selectedSeat != null)
                    {
                        bool isValidUser = false;
                        Employee selectedUser = null;

                        while (!isValidUser)
                        {
                            // TODO: select employee by employee ID / PRN
                            Console.WriteLine("Select an employee to book the seat for:");
                            for (int i = 0; i < users.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {users[i].Name}");
                            }

                            if (int.TryParse(Console.ReadLine(), out int userIndex) && userIndex > 0 && userIndex <= users.Count)
                            {
                                selectedUser = users[userIndex - 1];
                                isValidUser = true;

                                // Create a new booking
                                if(bookingService.BookSeatForEmployee(admin,selectedUser,bookingDate,seatNumber))
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Booking successful for {selectedUser.Name}! Seat Number: {selectedSeat.SeatNumber}, Date: {bookingDate.ToShortDateString()}");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.WriteLine("Error while booking!");
                                }                                
                            }
                            else
                            {
                                Console.ForegroundColor= ConsoleColor.Red;
                                Console.WriteLine("Invalid employee selection. Please try again.");
                                Console.ResetColor();
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid seat number. Please try again.");                            
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid seat number.");                        
                    Console.ResetColor();
                }
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid date format. Please enter a date in yyyy-MM-dd format.");
            Console.ResetColor();
        }
    }

    static void ModifyBooking(Employee admin,List<Employee> userList)
    {
        Console.Write("Enter the date of the booking to modify (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime bookingDate))
        {
            // Filter bookings on the specified date
            List<Booking> bookingsOnDate = bookingService.GetAllBookings(admin, bookingDate);

            if (bookingsOnDate.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No bookings found on the selected date.");                    
                Console.ResetColor();
                return;
            }
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Bookings on the selected date:");
            foreach (var booking in bookingsOnDate)
            {
                Employee selectedUser = null;
                foreach(var usertoselect in userList)
                {
                    if(booking.UserId == usertoselect.EmployeeId)
                    {
                        selectedUser = usertoselect;
                        break;
                    }
                }
                Console.WriteLine($"Booking ID: {booking.BookingId}, Seat Number: {booking.SeatNumber}, User: {selectedUser.Name}");
            }
            Console.ResetColor();

            // Select a booking to modify
            Console.Write("\nEnter the Booking ID to modify: ");
            if (int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Booking bookingToModify = null;
                foreach (var booking in bookingsOnDate)
                {
                    if (booking.BookingId == bookingId)
                    {
                        bookingToModify = booking;
                        break; 
                    }
                }
                if (bookingToModify != null)
                {
                    // Show available seats on the selected date
                    List<Seat> availableSeats = bookingService.GetSeatsOnGivenDay(bookingDate);
                    if(availableSeats.Count == 0)
                    {
                        Console.WriteLine("No seats avaialble for modification");
                        return;
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Available seats:");
                    foreach (var seat in availableSeats)
                    {
                        Console.WriteLine($"Seat Number: {seat.SeatNumber}");
                    }
                    Console.ResetColor();

                    // Choose a new seat
                    Console.Write("Enter the new seat number: ");
                    if (int.TryParse(Console.ReadLine(), out int newSeatNumber))
                    {
                        try
                        {
                            bookingService.ModifyAnyBooking(admin, bookingDate, bookingId, newSeatNumber);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Booking Modified successfully!");
                            Console.ResetColor();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Modification unsucessful");
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter a valid seat number.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Booking ID. Please try again.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid Booking ID.");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid date format. Please enter a date in yyyy-MM-dd format.");
            Console.ResetColor();
        }
    }

    static void ViewAllBookings(Employee admin, List<Employee> userList)
    {
        Console.Write("Enter the date to view bookings (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime bookingDate))
        {
            List<Booking> bookingsOnDate = bookingService.GetAllBookings(admin, bookingDate);

            if (bookingsOnDate.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No bookings found on the selected date.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Bookings on the selected date:");
            foreach (var booking in bookingsOnDate)
            {
                Employee selectedUser = null;
                foreach(Employee userToSelect in userList)
                {
                    if(booking.UserId == userToSelect.EmployeeId)
                    {
                        selectedUser= userToSelect;
                        break;
                    }
                }
                Console.WriteLine($"Booking ID: {booking.BookingId}, Seat Number: {booking.SeatNumber}, User: {selectedUser.Name}");
            }
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid date format. Please enter a date in yyyy-MM-dd format.");
            Console.ResetColor();
        }
    }

    static void DeleteBooking(Employee admin,List<Employee> userList)
    {
        Console.Write("Enter the date of the booking to delete (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime bookingDate))
        {
            List<Booking> bookingsOnDate = bookingService.GetAllBookings(admin,bookingDate);

            if (bookingsOnDate.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No bookings found on the selected date.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Bookings on the selected date:");
            foreach (var booking in bookingsOnDate)
            {
                Employee selectedUser = null;
                foreach(Employee userToSelect in userList)
                {
                    if(booking.UserId == userToSelect.EmployeeId)
                    {
                        selectedUser = userToSelect;
                        break;
                    }
                }
                Console.WriteLine($"Booking ID: {booking.BookingId}, Seat Number: {booking.SeatNumber}, User: {selectedUser.Name}");
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Enter the Booking ID to delete: ");
            Console.ResetColor();
            if (int.TryParse(Console.ReadLine(), out int bookingId))
            {
                try
                {
                    bookingService.CancelAnyBooking(admin, bookingId);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Booking cancelled successfully");
                    Console.ResetColor();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Booking couldn't be cancelled!");
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid Booking ID.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid date format. Please enter a date in yyyy-MM-dd format.");
            Console.ResetColor();
        }
    }
}
