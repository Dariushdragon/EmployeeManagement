namespace EmployeeService.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Department { get; private set; }
    public string Position { get; private set; }
    public DateTimeOffset EmploymentDate { get; private set; }
    public EmployeePreferences Preferences { get; private set; } = new();

    private Employee() { } // EF Core

    public Employee(Guid userId,
                    string department,
                    string position,
                    DateTimeOffset employmentDate,
                    EmployeePreferences preferences)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Department = department;
        Position = position;
        EmploymentDate = employmentDate;
        Preferences = preferences;
    }
    public static Employee Create()
    {
        return new Employee();
    }
    public void UpdateDetails(string department, string position, DateTimeOffset employmentDate)
    {
        Department = department;
        Position = position;
        EmploymentDate = employmentDate;
    }

    public void UpdatePreferences(EmployeePreferences preferences) => Preferences = preferences;
}
