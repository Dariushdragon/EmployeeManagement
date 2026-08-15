using EmployeeService.Domain.Entities;

namespace EmployeeService.Application.DTOs;

public record EmployeePreferencesDto(string? Language, string Theme ,bool? ReceivedEmail, bool? ReceiveSms);

public record CreateEmployeeDto(Guid UserId,
                                string Department,
                                DateTimeOffset EmploymentDate,
                                string Position,
                                EmployeePreferencesDto Preferences);

public record UpdateEmployeeDto(string Department,
                                DateTime EmploymentDate,
                                string Position);

public record EmployeeResponseDto(Guid Id,
                                  Guid UserId,
                                  string Department,
                                  DateTimeOffset EmploymentDate,
                                  string Position,
                                  string Status,
                                  EmployeePreferencesDto Preferences);

public record GetEmployeeQuery(int PageNumber = 1,
                               int PageSize = 10,
                               DateTime? FromDate = null,
                               DateTime? ToDate = null,
                               EmployeeStatus? Status = null);

public record PagedResult<T>(IReadOnlyList<T> Items, int PageNumber, int PageSize, int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
