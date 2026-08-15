using EmployeeService.Application.Interfaces;
using EmployeeService.Domain.Entities;
using EmployeeService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Infrastructure.Repositories;

public class ReservationRepository(EmployeeDbContext _context) : IReservationRepository
{
    public Task<Employee?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _context.Reservations.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<(IReadOnlyList<Employee> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate,
        EmployeeStatus? status, CancellationToken ct = default)
    {
        var query = _context.Reservations.AsNoTracking().AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(r => r.EmploymentDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(r => r.EmploymentDate <= toDate.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(r => r.Position)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task AddAsync(Employee reservation, CancellationToken ct = default)
    {
        _context.Reservations.AddAsync(reservation, ct);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
}
