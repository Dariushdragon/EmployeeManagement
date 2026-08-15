namespace EmployeeService.Application.Interfaces;

public interface INotificationServiceClient
{
    Task SendAsync(Guid userId, string title, string message, CancellationToken ct = default);
}
