﻿using BookingSystemProject.Models;
namespace BookingSystemProject.Services;

public interface IEmployeeService
{
    Employee CreateAnEmployee(string name, string email, RoleType role);
    Employee GetEmployeeByEmployeeId(int id);
    List<Employee> GetAllEmployees();
    void AddAnEmployee(Employee employee);
    bool UpdateAnEmployee(int id,string newName, string newEmail, RoleType newRole);
    void RemoveAnEmployee(Employee employee);

}
