using Agdata.SeatBookingSystem.Domain.Entities;

namespace Agdata.SeatBookingSystem.Application.Interfaces;

public interface IEmployeeService
{    
    Employee GetEmployeeByEmployeeId(int id);
    IEnumerable<Employee> GetAllEmployees();
    int AddAnEmployee(Employee employee);
    bool UpdateAnEmployee(int id, string newName, string newEmail, RoleType newRole);
    bool RemoveAnEmployee(int empId);

}
