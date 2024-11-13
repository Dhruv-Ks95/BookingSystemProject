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
    public bool RemoveEmployee(int employeeId)
    {
        _employees.RemoveAll(e => e.EmployeeId == employeeId);
        return true;
    }
    public Employee GetEmployeeById(int employeeId)
    {
        return _employees.Find(e => e.EmployeeId == employeeId);
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        return _employees;
    }

}
