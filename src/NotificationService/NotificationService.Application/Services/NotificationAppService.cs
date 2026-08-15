using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace NotificationService.Application.Services;

public class NotificationAppService : INotificationAppService
{
    private readonly INotificationRepository _repository;
    private readonly ILogger<NotificationAppService> _logger;

    public NotificationAppService(INotificationRepository repository, ILogger<NotificationAppService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto, CancellationToken ct = default)
    {
        var notification = new Notification(dto.UserId, dto.Title, dto.Message);
        await _repository.AddAsync(notification, ct);
        await _repository.SaveChangesAsync(ct);

        _logger.LogInformation("Notification {Id} created for user {UserId}: {Title}",
            notification.Id, notification.UserId, notification.Title);

        return Map(notification);
    }

    public async Task<NotificationResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var notification = await _repository.GetByIdAsync(id, ct);
        return notification is null ? null : Map(notification);
    }

    public async Task<PagedResult<NotificationResponseDto>> GetListAsync(GetNotificationsQuery query, CancellationToken ct = default)
    {
        var (items, total) = await _repository.GetPagedAsync(query.UserId, query.PageNumber, query.PageSize, ct);
        return new PagedResult<NotificationResponseDto>(items.Select(Map).ToList(), query.PageNumber, query.PageSize, total);
    }

    private static NotificationResponseDto Map(Notification n) => new(n.Id, n.UserId, n.Title, n.Message, n.CreatedAt);
}
