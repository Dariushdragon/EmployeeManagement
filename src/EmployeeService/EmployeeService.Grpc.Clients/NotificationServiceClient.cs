using Contracts.Grpc.Notification;
using EmployeeService.Application.Interfaces;

namespace EmployeeService.Grpc.Clients;

public class NotificationServiceClient(NotificationGrpc.NotificationGrpcClient _client) : INotificationServiceClient
{
    public async Task SendAsync(Guid userId, string title, string message, CancellationToken ct = default)
    {
        await _client.SendNotificationAsync(new SendNotificationRequest
        {
            UserId = userId.ToString(),
            Title = title,
            Message = message
        }, cancellationToken: ct);
    }
}
