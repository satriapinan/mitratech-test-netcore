using System;

public class Employee
{
    public int EmployeeID { get; set; }
    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }

    public Employee(int id, string name, DateTime birthDate)
    {
        EmployeeID = id;
        FullName = name;
        BirthDate = birthDate;
    }

    public override string ToString()
    {
        return $"Employee ID: {EmployeeID}, Name: {FullName}, Birth Date: {BirthDate:dd-MMM-yyyy}";
    }
}
