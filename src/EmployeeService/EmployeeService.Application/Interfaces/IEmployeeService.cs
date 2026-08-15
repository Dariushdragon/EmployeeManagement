using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto, CancellationToken ct = default);
    Task<EmployeeResponseDto?> UpdateAsync(Guid id, UpdateEmployeeDto dto, CancellationToken ct = default);
    Task<EmployeeResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<EmployeeResponseDto>> GetListAsync(GetEmployeeQuery query, CancellationToken ct = default);
    Task<EmployeeResponseDto?> UpdatePreferencesAsync(Guid id, EmployeePreferencesDto dto, CancellationToken ct = default);
}
