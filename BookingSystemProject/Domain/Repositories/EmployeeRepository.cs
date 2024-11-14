using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Interfaces;

namespace Agdata.SeatBookingSystem.Domain.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private List<Employee> _employees;

    public EmployeeRepository()
    {
        _employees = new List<Employee>();
    }

    public int AddEmployee(Employee employee)
    {
        _employees.Add(employee);
        return employee.EmployeeId;
    }
    public Employee GetEmployeeById(int employeeId)
    {
        return _employees.Find(e => e.EmployeeId == employeeId);
    }
    public bool RemoveEmployee(int employeeId)
    {
        Employee empToDelete = GetEmployeeById(employeeId);
        _employees.Remove(empToDelete);
        return true;
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        return _employees;
    }

}
