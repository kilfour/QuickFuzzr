using QuickPulse.Explains;

namespace QuickFuzzr.Tests._Tools.Models;

[CodeExample]
public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

[CodeExample]
public class Employee : Person
{
    public string Email { get; set; } = string.Empty;
    public string SocialSecurityNumber { get; set; } = string.Empty;
}

[CodeExample]
public class HousedEmployee : Employee
{
    public Address Address { get; set; } = new Address();
}

[CodeExample]
public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}