using FluentAssertions;
using BookingSystemProject.Application.Services;
using BookingSystemProject.Domain.Entities;
using BookingSystemProject.Domain.Repositories;

namespace BookingSystemProject.Tests;
public class EmployeeServiceTests
{
    private EmployeeService _employeeService;
    private EmployeeRepository _employeeRepository;

    public EmployeeServiceTests()
    {
        _employeeRepository = new EmployeeRepository();
        _employeeService = new EmployeeService(_employeeRepository);
    }

    [Fact]
    public void CreateAnEmployee_Should_Return_Employee_With_Correct_Details()
    {
        string name = "Dhruv";
        string email = "dhruv@company.com";
        RoleType role = RoleType.User;

        var result = _employeeService.CreateAnEmployee(name, email, role);

        result.Name.Should().Be(name);
        result.Email.Should().Be(email);
        result.Role.Should().Be(role);
    }

    [Fact]
    public void GetEmployeeByEmployeeId_Should_Return_Correct_Employee()
    {
        Employee employee = new Employee("Dhruv", "dhruv@company.com", RoleType.User);
        _employeeRepository.AddEmployee(employee);

        var result = _employeeService.GetEmployeeByEmployeeId(employee.EmployeeId);

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
        Employee employee1 = new Employee("dhruv", "dhruv@company.com", RoleType.Admin);
        Employee employee2 = new Employee("shukla", "shukla@company.com", RoleType.User);
        _employeeRepository.AddEmployee(employee1);
        _employeeRepository.AddEmployee(employee2);

        var result = _employeeService.GetAllEmployees();

        result.Should().Contain(new List<Employee> { employee1, employee2 });
    }

    [Fact]
    public void AddAnEmployee_Should_Add_Employee_To_Repository()
    {
        var employee = new Employee("dhruv", "dhruv@company.com", RoleType.User);

        _employeeService.AddAnEmployee(employee);

        _employeeRepository.GetEmployeeById(employee.EmployeeId).Should().Be(employee);
    }

    [Fact]
    public void UpdateAnEmployee_Should_Update_Employee_Details()
    {
        Employee employee = new Employee("dhruv", "dhruv@company.com", RoleType.User);
        _employeeRepository.AddEmployee(employee);
        string newName = "shukla";
        string newEmail = "shukla@company.com";
        RoleType newRole = RoleType.Admin;

        var result = _employeeService.UpdateAnEmployee(employee.EmployeeId, newName, newEmail, newRole);

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
        Employee employee = new Employee("dhruv", "dhruv@company.com", RoleType.User);
        _employeeRepository.AddEmployee(employee);

        _employeeService.RemoveAnEmployee(employee);

        _employeeRepository.GetEmployeeById(employee.EmployeeId).Should().BeNull();
    }
}
