using FluentAssertions;
using Agdata.SeatBookingSystem.Application.Services;
using Agdata.SeatBookingSystem.Domain.Entities;
using Agdata.SeatBookingSystem.Domain.Repositories;
using Agdata.SeatBookingSystem.Infrastructure.Data;
using Agdata.SeatBookingSystem.Tests.Mocks;

namespace Agdata.SeatBookingSystem.Tests;
public class EmployeeServiceTests
{
    SeatBookingDbContext dbContext = MockDbContext.GetDbContextWithTestData();
    private EmployeeService _employeeService;
    private EmployeeRepository _employeeRepository;

    public EmployeeServiceTests()
    {
        _employeeRepository = new EmployeeRepository(dbContext);
        _employeeService = new EmployeeService(_employeeRepository);
    }    

    [Fact]
    public void GetEmployeeByEmployeeId_Should_Return_Correct_Employee()
    {
        Employee employee = new Employee("Akshay K", "akshay@company.com", RoleType.User);
        int id = _employeeRepository.AddEmployee(employee);

        var result = _employeeService.GetEmployeeByEmployeeId(id);

        result.Should().Be(employee);
    }

    [Fact]
    public void GetEmployeeByEmployeeId_Should_Return_Null_If_Employee_Not_Found()
    {
        var result = _employeeService.GetEmployeeByEmployeeId(999);

        result.Should().BeNull();
    }

    [Fact]
    public void GetAllEmployees_Should_Return_All_Employees()
    {        
        var listOfEmps = _employeeService.GetAllEmployees();
        int result = listOfEmps.Count();
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public void AddAnEmployee_Should_Add_Employee_To_Repository()
    {
        var employee = new Employee("arnav", "arnav@company.com", RoleType.User);

        _employeeService.AddAnEmployee(employee);

        _employeeRepository.GetEmployeeById(employee.EmployeeId).Should().Be(employee);
    }

    [Fact]
    public void UpdateAnEmployee_Should_Update_Employee_Details()
    {
        Employee employee = new Employee("das", "das@company.com", RoleType.User);
        int id = _employeeRepository.AddEmployee(employee);
        string newName = "shukla";
        string newEmail = "shukla@company.com";
        RoleType newRole = RoleType.Admin;

        var result = _employeeService.UpdateAnEmployee(id, newName, newEmail, newRole);

        result.Should().BeTrue();
        employee.Name.Should().Be(newName);
        employee.Email.Should().Be(newEmail);
        employee.Role.Should().Be(newRole);
    }

    [Fact]
    public void UpdateAnEmployee_Should_Return_False_If_Employee_Not_Found()
    {
        var result = _employeeService.UpdateAnEmployee(999, "Nobody", "nobody@company.com", RoleType.User);

        result.Should().BeFalse();
    }

    [Fact]
    public void RemoveAnEmployee_Should_Remove_Employee_From_Repository()
    {
        Employee employee = new Employee("dhruvk", "kdhruv@company.com", RoleType.User);
        _employeeRepository.AddEmployee(employee);

        _employeeService.RemoveAnEmployee(employee.EmployeeId);

        _employeeRepository.GetEmployeeById(employee.EmployeeId).Should().BeNull();
    }
}
