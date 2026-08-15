using Common.Exceptions;
using EmployeeService.Application.DTOs;
using EmployeeService.Application.Interfaces;
using EmployeeService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EmployeeService.Application.Services;

public class ReservationService(IReservationRepository repository,
                                IIdentityServiceClient identityClient,
                                INotificationServiceClient notificationClient,
                                ILogger<ReservationService> logger) : IEmployeeService
{
    public async Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto, CancellationToken ct = default)
    {
        var userExists = await identityClient.UserExistsAsync(dto.UserId, ct);
        if (!userExists)
            throw new DependencyNotFoundException($"کاربر با شناسه {dto.UserId} یافت نشد.");

        var preferences = new EmployeePreferences
        {
            Language = dto.Preferences.Language,
            Theme = "Dark",
            ReceiveEmail = dto.Preferences.ReceiveSms
        };

        var reservation = new Employee(dto.UserId, dto.Department, dto.Position, dto.EmploymentDate, preferences);

        await repository.AddAsync(reservation, ct);
        await repository.SaveChangesAsync(ct);

        // Notification is best-effort: failure must NOT roll back the reservation.
        try
        {
            await notificationClient.SendAsync(
                reservation.UserId,
                "رزرو شما ثبت شد",
                $"میز {reservation.Department} برای {reservation.Position:yyyy-MM-dd HH:mm} رزرو شد.",
                ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to send notification for reservation {ReservationId}. Reservation was NOT rolled back.",
                reservation.Id);
        }

        return Map(reservation);
    }

    public async Task<EmployeeResponseDto?> UpdateAsync(Guid id, UpdateEmployeeDto dto, CancellationToken ct = default)
    {
        var reservation = await repository.GetByIdAsync(id, ct);
        if (reservation is null)
            return null;

        reservation.UpdateDetails(dto.Department, dto.Position, dto.EmploymentDate);
        await repository.SaveChangesAsync(ct);
        return Map(reservation);
    }

    public async Task<EmployeeResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var reservation = await repository.GetByIdAsync(id, ct);
        return reservation is null ? null : Map(reservation);
    }

    public async Task<PagedResult<EmployeeResponseDto>> GetListAsync(GetEmployeeQuery query, CancellationToken ct = default)
    {
        var (items, total) = await repository.GetPagedAsync(
            query.PageNumber, query.PageSize, query.FromDate, query.ToDate, query.Status, ct);

        return new PagedResult<EmployeeResponseDto>(items.Select(Map).ToList(), query.PageNumber, query.PageSize, total);
    }

    public async Task<EmployeeResponseDto?> UpdatePreferencesAsync(Guid id, EmployeePreferencesDto dto, CancellationToken ct = default)
    {
        var reservation = await repository.GetByIdAsync(id, ct);
        if (reservation is null)
            return null;

        reservation.UpdatePreferences(new EmployeePreferences
        {
            Language = dto.Language,
            Theme = dto.Theme,
            ReceiveEmail = dto.ReceivedEmail
        });

        await repository.SaveChangesAsync(ct);
        return Map(reservation);
    }

    private static EmployeeResponseDto Map(Employee r) => new(r.Id, r.UserId, r.Department, r.EmploymentDate, r.Position,"",
        new EmployeePreferencesDto(r.Preferences.Language, r.Preferences.Theme, r.Preferences.ReceiveEmail, r.Preferences.ReceiveSms));
}
