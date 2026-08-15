using Contracts.Grpc.Notification;
using Grpc.Core;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;

namespace NotificationService.Api.Grpc;

public class NotificationGrpcService(INotificationAppService _notificationService, ILogger<NotificationGrpcService> _logger) : NotificationGrpc.NotificationGrpcBase
{
    public override async Task<SendNotificationResponse> SendNotification(SendNotificationRequest request,
                                                                          ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid userId format."));

        var result = await _notificationService.CreateAsync(
            new CreateNotificationDto(userId, request.Title, request.Message),
            context.CancellationToken);

        _logger.LogInformation("Notification event received via gRPC for user {UserId}", userId);

        return new SendNotificationResponse { Success = true, NotificationId = result.Id.ToString() };
    }
}
