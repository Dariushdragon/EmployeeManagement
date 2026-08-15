using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories;

public class UserRepository(IdentityDbContext _context) : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default) =>
        _context.Users.AnyAsync(u => u.Id == id, ct);

    public async Task<(IReadOnlyList<User> Items, int TotalCount)> GetPagedAsync(int pageNumber,
                                                                                 int pageSize,
                                                                                 bool? isActive,
                                                                                 CancellationToken ct = default)
    {
        var query = _context.Users.AsNoTracking().AsQueryable();

        if (isActive.HasValue)
            query = query.Where(u => u.IsActive == isActive.Value);

        var total = await query.CountAsync(ct);
        var items = await query.OrderBy(u => u.FullName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return (items, total);
    }

    public Task AddAsync(User user, CancellationToken ct = default)
    {
        _context.Users.AddAsync(user, ct);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
}
