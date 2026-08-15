namespace NotificationService.Application.DTOs;

public record CreateNotificationDto(Guid UserId, string Title, string Message);

public record NotificationResponseDto(Guid Id, Guid UserId, string Title, string Message, DateTime CreatedAt);

public record PagedResult<T>(IReadOnlyList<T> Items, int PageNumber, int PageSize, int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}

public record GetNotificationsQuery(Guid? UserId, int PageNumber = 1, int PageSize = 10);
