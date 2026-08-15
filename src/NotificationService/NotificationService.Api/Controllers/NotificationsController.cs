using Common;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;

namespace NotificationService.Api.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController(INotificationAppService _notificationService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _notificationService.GetByIdAsync(id, ct);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail("اعلان یافت نشد."));

        return Ok(ApiResponse<NotificationResponseDto>.Ok(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] Guid? userId = null,
                                             [FromQuery] int pageNumber = 1,
                                             [FromQuery] int pageSize = 10,
                                             CancellationToken ct = default)
    {
        var result = await _notificationService.GetListAsync(new GetNotificationsQuery(userId, pageNumber, pageSize), ct);
        return Ok(ApiResponse<PagedResult<NotificationResponseDto>>.Ok(result));
    }
}
