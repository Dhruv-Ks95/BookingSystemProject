namespace Agdata.SeatBookingSystem.Domain.Entities;

public class Employee
{
    //private static int nextId = 1;
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public RoleType Role { get; set; }

    public Employee(string name, string email, RoleType role)
    {
        //EmployeeId = nextId++;
        Name = name;
        Email = email;
        Role = role;
    }

    public Employee() { }
}
