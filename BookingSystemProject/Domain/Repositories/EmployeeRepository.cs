using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Interfaces;
using Agdata.SeatBookingSystem.Infrastructure.Data;

namespace Agdata.SeatBookingSystem.Domain.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private SeatBookingDbContext context;

    public EmployeeRepository(SeatBookingDbContext _context)
    {
        context = _context;
    }

    public int AddEmployee(Employee employee)
    {
        context.Employees.Add(employee);
        context.SaveChanges();
        return employee.EmployeeId;
    }
    public Employee GetEmployeeById(int employeeId)
    {
        Employee employee = context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
        return employee;
    }
    public bool RemoveEmployee(int employeeId)
    {
        var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
        if (employee != null)
        {
            context.Employees.Remove(employee);
            var rowsAffected = context.SaveChanges();
            return rowsAffected > 0;
        }
        return false;
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        return context.Employees.ToList();
    }

}
