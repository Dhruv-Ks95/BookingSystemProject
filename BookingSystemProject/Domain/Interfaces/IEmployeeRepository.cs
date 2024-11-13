using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Domain.Interfaces;

public interface IEmployeeRepository
{
    int AddEmployee(Employee employee);
    bool RemoveEmployee(int employeeId); 
    Employee GetEmployeeById(int employeeId);
    IEnumerable<Employee> GetAllEmployees();
}
