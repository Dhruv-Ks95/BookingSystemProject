using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agdata.SeatBookingSystem.Tests.Mocks;

public static class MockDbContext
{
    public static SeatBookingDbContext GetDbContextWithTestData()
    {
        var options = new DbContextOptionsBuilder<SeatBookingDbContext>().UseInMemoryDatabase(databaseName: "SeatBookingDbTest").Options;

        var dbContext = new SeatBookingDbContext(options);

        // Create and insert mocking data
        dbContext.Employees.AddRange(new List<Employee>
        {
            new Employee { Name = "Admin", Email = "admin@company.com", Role = RoleType.Admin },
            new Employee { Name = "Dhruv Ks", Email = "dhruv@company.com", Role = RoleType.User },
            new Employee { Name = "Aradhya S", Email = "aradhya@company.com", Role = RoleType.User },
            new Employee { Name = "Srijan D", Email = "srijan@company.com", Role = RoleType.User },
            new Employee { Name = "Dileep R", Email = "dileep@company.com", Role = RoleType.User }

        });

        dbContext.Seats.AddRange(new List<Seat>
        {
            new Seat { SeatNumber = 1 },
            new Seat { SeatNumber = 2 },
            new Seat { SeatNumber = 3 },
            new Seat { SeatNumber = 4 },
            new Seat { SeatNumber = 5 },
            new Seat { SeatNumber = 6 },
            new Seat { SeatNumber = 7 },
            new Seat { SeatNumber = 8 },
            new Seat { SeatNumber = 9 },
            new Seat { SeatNumber = 10 }
        });

        DateTime date = DateTime.Now.Date.AddDays(4);
        dbContext.Bookings.AddRange(new List<Booking>
        {
            new Booking(2, date, 10),
            new Booking(4, date, 9)
        });

        dbContext.SaveChanges();
        return dbContext;
    }
}
