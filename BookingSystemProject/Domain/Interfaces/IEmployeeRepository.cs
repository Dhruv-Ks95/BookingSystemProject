using BookingSystemProject.Domain.Entities;

namespace BookingSystemProject.Domain.Interfaces;

public interface IEmployeeRepository
{
    void AddEmployee(Employee employee);
    void RemoveEmployee(int employeeId);
    Employee GetEmployeeById(int employeeId);
    List<Employee> GetAllEmployees();
}
