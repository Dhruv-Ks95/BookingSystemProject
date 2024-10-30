using BookingSystemProject.Models;
using BookingSystemProject.Repository;
namespace BookingSystemProject.Services;

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

    public List<Employee> GetAllEmployees()
    {
        return employeeRepository.GetAllEmployees();
    }

    public void AddAnEmployee(Employee employee)
    {
        employeeRepository.AddEmployee(employee);
    }

    public bool UpdateAnEmployee(int id, string newName, string newEmail, RoleType newRole)
    {
        Employee emp = employeeRepository.GetEmployeeById(id);
        if(emp != null)
        {
            emp.Name = newName;
            emp.Email = newEmail;
            emp.Role = newRole;
            return true;
        }
        return false;
    }

    public void RemoveAnEmployee(Employee employee)
    {
        employeeRepository.RemoveEmployee(employee.EmployeeId);
    }
}
