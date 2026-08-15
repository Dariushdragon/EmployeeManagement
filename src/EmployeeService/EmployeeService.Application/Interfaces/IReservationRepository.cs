using EmployeeService.Domain.Entities;

namespace EmployeeService.Application.Interfaces;

public interface IReservationRepository
{
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<Employee> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate,
        EmployeeStatus? status, CancellationToken ct = default);
    Task AddAsync(Employee reservation, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
