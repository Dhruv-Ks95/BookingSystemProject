using BookingSystemProject.Models;
namespace BookingSystemProject.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private List<Employee> _employees;

    public EmployeeRepository()
    {
        _employees = new List<Employee>();
    }

    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
    }
    public void RemoveEmployee(int employeeId)
    {
        _employees.RemoveAll(e =>  e.EmployeeId == employeeId);
    }
    public Employee GetEmployeeById(int employeeId)
    {
        return _employees.Find(e => e.EmployeeId == employeeId);
    }

    public List<Employee> GetAllEmployees()
    {
        return _employees;
    }

}
