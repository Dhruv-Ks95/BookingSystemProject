using BookingSystemProject.Models;
namespace BookingSystemProject.Repository;

public interface IEmployeeRepository
{
    void AddEmployee(Employee employee);
    void RemoveEmployee(int employeeId);
    Employee GetEmployeeById(int employeeId);
    List<Employee> GetAllEmployees();
}
