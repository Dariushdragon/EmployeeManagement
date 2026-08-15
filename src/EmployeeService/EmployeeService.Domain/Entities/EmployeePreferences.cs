namespace EmployeeService.Domain.Entities;

// Value Object - persisted as jsonb in PostgreSQL
public class EmployeePreferences
{
    public string? Language { get; set; }
    public string Theme { get; set; }
    public bool? ReceiveEmail { get; set; }
    public bool? ReceiveSms { get; set; }
}
