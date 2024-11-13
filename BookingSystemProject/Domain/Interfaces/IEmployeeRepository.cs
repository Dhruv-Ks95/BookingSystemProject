using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Domain.Interfaces;

public interface IEmployeeRepository
{
    void AddEmployee(Employee employee); // return ID
    void RemoveEmployee(int employeeId); // return boolean
    Employee GetEmployeeById(int employeeId);
    IEnumerable<Employee> GetAllEmployees(); // change to Enums
}
