using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Persistence;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _context;

    public NotificationRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _context.Notifications.FirstOrDefaultAsync(n => n.Id == id, ct);

    public async Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetPagedAsync(
        Guid? userId, int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var query = _context.Notifications.AsNoTracking().AsQueryable();

        if (userId.HasValue)
            query = query.Where(n => n.UserId == userId.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task AddAsync(Notification notification, CancellationToken ct = default)
    {
        _context.Notifications.AddAsync(notification, ct);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
}
