using Agdata.SeatBookingSystem.Application.Interfaces;
using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Interfaces;

namespace Agdata.SeatBookingSystem.Application.Services;

public class EmployeeService : IEmployeeService
{
    private IEmployeeRepository employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepo)
    {
        employeeRepository = employeeRepo;
    }

    public Employee CreateAnEmployee(string name, string email, RoleType role)
    {
        return new Employee(name, email, role);
    }

    public Employee GetEmployeeByEmployeeId(int id)
    {
        return employeeRepository.GetEmployeeById(id); // Can be null
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        return employeeRepository.GetAllEmployees();
    }

    public int AddAnEmployee(Employee employee)
    {
        employeeRepository.AddEmployee(employee);
        return employee.EmployeeId;
    }

    public bool UpdateAnEmployee(int id, string newName, string newEmail, RoleType newRole)
    {
        Employee emp = employeeRepository.GetEmployeeById(id);
        if (emp != null)
        {
            emp.Name = newName;
            emp.Email = newEmail;
            emp.Role = newRole;
            return true;
        }
        return false;
    }

    public bool RemoveAnEmployee(int empid)
    {
        employeeRepository.RemoveEmployee(empid);
        return true;
    }
}
