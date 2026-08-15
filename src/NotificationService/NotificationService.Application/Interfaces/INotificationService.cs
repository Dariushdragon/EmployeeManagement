using NotificationService.Application.DTOs;

namespace NotificationService.Application.Interfaces;

public interface INotificationAppService
{
    Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto, CancellationToken ct = default);
    Task<NotificationResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<NotificationResponseDto>> GetListAsync(GetNotificationsQuery query, CancellationToken ct = default);
}
